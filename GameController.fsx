#load "AsyncEventQueue.fsx"
#load "Gui.fsx"
#load "HeapsZipper.fsx"

open AsyncEventQueue
open Gui
open HeapsZipper


type GameState =
    { TurnBit : bool
    ; HeapsZipper : HeapsZipper.HeapsZipper
    ; AiEnabled : bool // kan laves til et modul
    ; Write : string
    }


let initialState =
    { TurnBit = true
    ; HeapsZipper = HeapsZipper.initialModel // kun til tests
    ; AiEnabled = false
    ; Write = "1"
    }


// for parsing Int active patterns
// denne skal ind i parsers sammen med den anden fra gui
let (|Int|_|) str =
    match System.Int32.TryParse(str) with
        | (true,int) -> Some(int)
        | _ -> None


let (|Win|_|) x =
    match HeapsZipper.focus x with
        | x when x = 0 ->
            Some(x)
        | _ -> None


let q = AsyncEventQueue.instance


let rec menu() =
    async {
        let state = initialState

        // Gui.update state
        // kun gui variable skal komme fra gui
        // update bør ske på det step man er ved.. lidt æregeligt med hvordan vi chekker if someone won

        let! msg = q.Receive()

        match msg with
            | AsyncEventQueue.NewGame ->
                return! draw(state)
            | _ ->
                failwith "Menu: Unexpected Message"
    }

//in these without listen you should disable all buttons... dissekan nok flyttes ud
and checkWin(initialState) =

    let state = initialState

    match state with

        | { HeapsZipper = Win _ } ->
            win(state)

        | _ ->
            switch(state)


and switch(initialState) =

    let state = initialState

    let turnBit =
          not initialState.TurnBit

    let state =
        { initialState with TurnBit = turnBit }

    draw(state)

and draw(initialState) =

    //Gui.clearHeapsZipper
    //denne function skal opdateres vi skal undnytte zipper
    Gui.clearHeapsZipper()
    Gui.drawHeapsFromList(HeapsZipper.toIndexedList initialState.HeapsZipper)
    Gui.setWrite(initialState.Write)

    let state = initialState

    ready(state)


and ready(initialState) =
    async {

        let! msg = q.Receive()

        match msg with

            | AsyncEventQueue.Take ->

                // måske gå til switch state
                let state = initialState

                match state with

                    | { Write = Int b } ->
                        let heapsZipper =
                            HeapsZipper.subtract b initialState.HeapsZipper

                        // det her er en fucked structur
                        let state =
                          { initialState with HeapsZipper = heapsZipper }

                        return! checkWin(state)

                    | _ ->
                        failwith "Write: Unexpected Message"

            | AsyncEventQueue.SelectHeap n ->

                let heapsZipper =
                    HeapsZipper.move n initialState.HeapsZipper

                let state =
                    { initialState with HeapsZipper = heapsZipper }


                return! draw(state)

            | AsyncEventQueue.Write str ->

                let state = { initialState with Write = str }

                match state with

                    | { Write = Int b } ->
                        return! ready(state)

                    | _ ->
                        failwith "Write: Unexpected Message"

            //| NewAiGame       -> return! ready(setupNewGame(true))
            | _ ->
                failwith "Ready: Unexpected Message"
    }




and aiReady(gameState) =
    async {
        failwith "Not implemented"
        (*
        printfn "IN AI MAKE MOVE"

        if Heap.toInt (HeapsZipper.focus gameState.Heap) = 0 then
            return! winGame(gameState)
        else
        Gui.update (HeapsZipper.toList gameState.Heap)

        let moveTupl = Ai.aiMove (HeapsZipper.toList gameState.Heap)
        printfn "AiMove: %A" moveTupl
        //Gui.colorNthRadio (fst moveTupl)
        let ns = move moveTupl gameState
        return! ready(ns)
        *)
    }

and win(gamestate) =
    async {
        failwith "win not implemented"
    }
