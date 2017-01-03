module GameController

    #load "AsyncEventQueue.fsx"
    #load "Gui.fsx"
    #load "HeapsZipper.fsx"
    #load "Ai.fsx"

    open Gui
    open AsyncEventQueue
    open HeapsZipper
    open Ai

    let rand = System.Random()
    type GameState = {
        TurnBit: bool;
        Heap: int list;
        AiEnabled: bool
    }

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
            AiEnabled = gs.AiEnabled
        }


    let setupNewGame (enableAI) = {
            TurnBit = if rand.Next(0, 1) = 1 then true else false;
            Heap = [for i in 1 .. rand.Next(3, 15) -> rand.Next(1, 100)];
            AiEnabled = enableAI;
        }

    let q = AsyncEventQueue.instance
    let rec menu() =
        async {

            let! msg = q.Receive()
            match msg with
                | NewGame   ->
                    let ng = setupNewGame(false)
                    printfn "%A" ng.Heap
                    return! ready(ng)
                | NewAiGame ->
                    let ng = setupNewGame(true)
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
            // Evt make color change to indicate ready.
            Gui.update gameState.Heap
            let! msg = q.Receive()
            match msg with
                | Move (a,b)    ->
                    let newGameState = move (a,b) gameState
                    if gameState.AiEnabled then return! aiReady(newGameState)
                    else return! ready(newGameState)
                | Clear         ->
                    return! menu()
                | NewGame         -> return! ready(setupNewGame(false))
                | NewAiGame       -> return! ready(setupNewGame(true))
                | _                         -> failwith "Ready: Unexpected Message"
        }
    and aiReady(gameState) =
        async {
            // Evt make color change to indicate ready.
            printfn "IN AI MAKE MOVE"
            Gui.update gameState.Heap

            let moveTupl = Ai.aiMove gameState.Heap
            printfn "AiMove: %A" moveTupl
            let ns = move moveTupl gameState
            return! ready(ns)
        }
    and winGame(gameState) =
        async {
            failwith "Not implemented"
        }



//    let initGame() =
//        let initState = {
//            TurnBit = rand.Next(0, 1);
//            Heap = [for i in 1 .. rand.Next(1, 5) -> rand.Next(1, 100)];
//            SelectedHeap = 0;
//            RemoveCount = 1
//        }
//
//        Async.StartImmediate (ready(initState))
