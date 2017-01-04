module GameController

    #load "types.fsx"
    #load "AsyncEventQueue.fsx"
    #load "Gui.fsx"
    #load "HeapsZipper.fsx"
    #load "Ai.fsx"

    open Types
    open Gui
    open AsyncEventQueue
    open HeapsZipper
    open Ai

    let rand = System.Random()

    //let mutable gameState = {}

    let rec remove i l =
        match i, l with
        | 0, x::xs -> xs
        | i, x::xs -> x::remove (i - 1) xs
        | i, [] -> failwith "index out of range"

    let rec subtract i c l =
        match i, l with
        | 0, x::xs -> (x - c)::xs
        | i, x::xs -> x::subtract (i - 1) c xs
        | i, [] -> failwith "index out of range"

    let move (a,b) gs =
        let newHeap =
            if gs.Heap.Item(a) - b <= 0
            then
                remove a gs.Heap
            else
                subtract a b gs.Heap
        in {
            TurnBit = not gs.TurnBit;
            Heap = newHeap;
            Ai = gs.Ai
        }

    (* Finds the next play-move for the AI
     *)
    let aiMove gameState =
        let m = findm gameState.Heap

        let move =
            if m = 0
            then (maxIndexBy gameState.Heap, 1)
            else (findMove gameState.Heap m)
        in
            match gameState.Ai with
            | NoAI      -> failwith "There is no AI!"
            | Easy      ->
                if rand.Next(1, 100) > 25
                then (fst move, snd move + rand.Next(1, 5))
                else move
            | Medium    ->
                if rand.Next(1, 100) > 50
                then (fst move, snd move + rand.Next(1, 5))
                else move
            | Hard      ->
                if rand.Next(1, 100) > 75
                then (fst move, snd move + rand.Next(1, 5))
                else move
            | Godlike   -> move

    let checkWin gs =
        gs.Heap.Length < 1

    let setupNewGame (aiState) = {
            TurnBit = if rand.Next(0, 1) = 1 then true else false;
            Heap = [for i in 1 .. rand.Next(2, 4) -> rand.Next(1, 10)];
            Ai = aiState
        }

    let q = AsyncEventQueue.instance
    let rec menu() =
        async {

            Gui.toggleDrawing false

            Gui.showButtons [Gui.newAiGameButton;Gui.newGameButton]
            Gui.hideButtons [Gui.clearButton]
            //Gui.disableButtons [Gui.newGameButton;Gui.newAiGameButton]

            let! msg = q.Receive()
            match msg with
                | NewGame   ->
                    let ng = setupNewGame(NoAI)
                    Gui.addLogMessage "New Game Started"
                    printfn "%A" ng.Heap
                    return! ready(ng)
                | NewAiGame ->
                    let ng = setupNewGame(Godlike)
                    Gui.addLogMessage "New AI Game Started"
                    if ng.TurnBit
                    then
                        return! aiReady(ng)
                    else
                        return! ready(ng)
                | _         -> failwith "Menu: Unexpected Message"
            // Evt make color change to indicate ready.
        }
    and ready(gameState) =
        async {
            if checkWin gameState
            then return! winGame(gameState)
            else

            // Evt make color change to indicate ready.
            Gui.update gameState.Heap
            Gui.hideButtons [Gui.newAiGameButton;Gui.newGameButton]
            Gui.showButtons [Gui.clearButton]
            //Gui.toggleDrawing true
            let! msg = q.Receive()
            match msg with
                | Move (a,b)    ->
                    //let logMsg = sprintf "Move: (%d,%d)" a b
                    Gui.addLogMessage (sprintf "Move: (%d,%d)" a b)

                    let newGameState = move (a,b) gameState

                    match gameState.Ai with
                    | NoAI  -> return! ready(newGameState)
                    | _     -> return! aiReady(newGameState)
                | Clear         ->
                    Gui.update []
                    return! menu()
                | _             -> failwith "Ready: Unexpected Message"
        }
    and aiReady(gameState) =
        async {
            if checkWin gameState
            then return! winGame(gameState)
            else

            // Evt make color change to indicate ready.
            Gui.update gameState.Heap
            let moveTupl = aiMove gameState

            printfn "AiMove: %A" moveTupl
            Gui.addLogMessage (sprintf "AI Move: %A" moveTupl)
            let ns = move moveTupl gameState
            return! ready(ns)
        }
    and winGame(gameState) =
        async {
            Gui.update gameState.Heap
            Gui.toggleDrawing false

            if not gameState.TurnBit
            then Gui.logWindow.Text <- (Gui.logWindow.Text + "\nPlayer 2 (or AI) has won!")
            else Gui.logWindow.Text <- (Gui.logWindow.Text + "\nPlayer 1 has won!")
            //Gui.win gameState
            return! menu()
        }
