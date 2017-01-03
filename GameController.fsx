#load "AsyncEventQueue.fsx"
#load "Gui.fsx"
#load "HeapsZipper.fsx"

open AsyncEventQueue
open Gui
open HeapsZipper


type GameState =
    { TurnBit: bool
    ; HeapsZipper: HeapsZipper.HeapsZipper
    ; AiEnabled: bool // kan laves til et modul
    }


let initialState =
    { TurnBit = true
    ; HeapsZipper = HeapsZipper.initialModel // kun til tests
    ; AiEnabled = false;
    }


let q = AsyncEventQueue.instance

let rec menu() =
    async {
        let state = initialState

        // Gui.update state
        // kun gui variable skal komme fra gui
        // update bør ske på det step man er ved.. lidt æregeligt med hvordan vi chekker if someone won

        Gui.clearHeapsZipper()

        let! msg = q.Receive()

        match msg with
            | AsyncEventQueue.NewGame ->
                return! ready(state)
            | _ ->
                failwith "Menu: Unexpected Message"
    }

and ready(initialState) =
    async {

        //Gui.clearHeapsZipper
        Gui.drawHeapsFromList(HeapsZipper.toList initialState.HeapsZipper)

        let! msg = q.Receive()

        match msg with
            | AsyncEventQueue.Take n ->

                let heapsZipper =
                    HeapsZipper.subtract n initialState.HeapsZipper

                let turnBit =
                    not initialState.TurnBit

                // det her er en fucked structur
                let state = { initialState with HeapsZipper = heapsZipper; TurnBit = turnBit }

                return! ready(state)

            | AsyncEventQueue.NewGame ->
                return! menu()
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

and winGame(gameState) =
    async {
        failwith "Not implemented"
    }
