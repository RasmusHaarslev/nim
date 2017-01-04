#load "AsyncEventQueue.fsx"
#load "Gui.fsx"
#load "HeapsZipper.fsx"
#load "Ai.fsx"
#load "Helpers.fsx"

open AsyncEventQueue
open Gui
open HeapsZipper
open Ai
open Helpers

// kunne måske godt tænke mig at have knapper defineret på gamestate..
type GameState =
    { TurnBit : bool
    ; HeapsZipper : HeapsZipper.HeapsZipper
    ; Ai : Ai.Ai option
    ; Write : string  // bør også være zipper... der rer altid en write
    /// kør HeapZipper helt i bund så mellem hver runde
    ; Log : string list // unsafe da jeg benytter hd.. kan være optional?... der er altid en log
    }

(*
type State =
  | State of GameState
for more modular code
*)

let initialAiGame =
    { TurnBit = true
    ; HeapsZipper = HeapsZipper.initialModel
    ; Ai = Some (Ai.initImprovedAI 2)
    ; Write = "1"
    ; Log = ["Game has just started"]
    }


let initialMultiplayerGame =
    { TurnBit = true
    ; HeapsZipper = HeapsZipper.initialModel
    ; Ai = None
    ; Write = "1"
    ; Log = ["Game has just started"]
    }


let q = AsyncEventQueue.instance


let rec menu() =
    async {

        //kunne have været fedt at binde buttons til state
        // lidt æregeligt med hvordan vi chekker if someone won
        Gui.clearChooseMatches()
        Gui.clearHeapsZipper()
        Gui.clearLog()

        Gui.showMenu(false)
        Gui.showNewGameButton(true)
        Gui.showNewAiGameButton(true)
        Gui.showDrawButton(false)

        let! msg = q.Receive()

        match msg with
            | AsyncEventQueue.NewGame ->
                let state = initialMultiplayerGame

                return! checkWin(state)
            | AsyncEventQueue.NewAiGame ->
                let state = initialAiGame

                return! checkWin(state)
            | _ ->
                failwith "Menu: Unexpected Message"
    }

//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
//in these without listen you should disable all buttons... dissekan nok flyttes ud
and checkWin(initialState) =

    let state = initialState

    match state with

        | { HeapsZipper = Helpers.Win _ } ->
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

    let state = initialState

    Gui.clearHeapsZipper()
    Gui.drawHeapsFromList(HeapsZipper.toIndexedList state.HeapsZipper)

    Gui.clearChooseMatches()
    Gui.initChooseMatches(initialState.Write)

    Gui.clearLog()
    Gui.initLog(List.head state.Log)

    aiReady(state)

// det giver måske ikke så meget mening man går direkte ind i denne her...
and aiReady(initialState) =

  let state = initialState

  match state with
     // vi vælger om turnbit starter på false eller true
      | { Ai = Some ai; TurnBit = a; HeapsZipper = h; Log = l } when a ->

          // kan ikke lave bad ai move sådan her
          let (i, j) = Ai.move h ai

          let heapsZipper =
              HeapsZipper.subtract2 i j h

          //ideally we would like only to add to log 1 place in code
          let log = (sprintf "Move:(%d,%d)" i j) :: l

          let state =
              { state with HeapsZipper = heapsZipper; Log = log }

          checkWin(state)
      | _ ->
          ready(state)

and ready(initialState) =
    async {

        Gui.showMenu(true)
        Gui.showNewGameButton(false)
        Gui.showNewAiGameButton(false)
        Gui.showDrawButton(true)

        let! msg = q.Receive()

        match msg with

            | AsyncEventQueue.Take ->

                let state = initialState

                match state with

                    | { Write = Helpers.Int b; HeapsZipper = h; Log = l} ->
                        let heapsZipper =
                            HeapsZipper.subtract b h

                            //ideally we would like only to add to log 1 place in code
                            // problemet er zipper jo ikke er indexeret så man bør give
                        let log = (sprintf "Player took:(%d)" b ) :: l

                        let state =
                            { initialState
                                with
                                    HeapsZipper = heapsZipper;
                                    Log = log
                            }

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

                    | { Write = Helpers.Int b } ->
                        return! ready(state)

                    | _ ->
                        failwith "bad write: not handled yet"

            | AsyncEventQueue.Menu ->
                return! menu()
            | _ ->
                failwith "Ready: Unexpected Message"
    }

and win(gamestate) =
    async {
      // add til log og gå til menu
        failwith "win not implemented"
    }
