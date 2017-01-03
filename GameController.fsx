module GameController
    
    #load "AsyncEventQueue.fsx"
    #load "Gui.fsx"
    #load "HeapsZipper.fsx"

    open Gui
    open AsyncEventQueue
    open HeapsZipper

    let rand = System.Random()
    type GameState = {
        TurnBit: bool;
        Heap: HeapsZipper.HeapZipper;
        AiEnabled: bool
    }

    //let mutable gameState = {}

    let move (a,b) gs = { 
            TurnBit = (not gs.TurnBit);
            Heap =  (HeapsZipper.subtract a b gs.Heap);
            AiEnabled = gs.AiEnabled;
        }

    let setupNewGame () = {
            TurnBit = if rand.Next(0, 1) = 1 then true else false;
            Heap = HeapsZipper.initFromList [for i in 1 .. rand.Next(1, 5) -> rand.Next(1, 100)];
            AiEnabled = false;
        }

    let q = AsyncEventQueue.instance
    let rec menu() =
        async {
            let ng = setupNewGame()
            let! msg = q.Receive()
            match msg with
                | NewGame   -> 
                    Gui.update (HeapsZipper.toList ng.Heap)
                    printfn "%A" (HeapsZipper.toList ng.Heap)
                    return! ready(ng)
                | NewAiGame -> if ng.TurnBit then return! aiReady(ng) else return! ready(ng)
                | _         -> failwith "Menu: Unexpected Message"
            // Evt make color change to indicate ready.
        }
    and ready(gameState) = 
        async {
            // Evt make color change to indicate ready.
            Gui.update (HeapsZipper.toList gameState.Heap)
            let! msg = q.Receive()
            match msg with
                | Move (a,b)    -> 
                    let newGameState = move (a,b) gameState
                    if gameState.AiEnabled then return! aiReady(newGameState)
                    else return! ready(newGameState)
                | Clear         ->
                    return! menu()
                | NewGame       -> return! ready(setupNewGame())
                | _             -> failwith "Ready: Unexpected Message"
        }
    and aiReady(gameState) =
        async {
            // Evt make color change to indicate ready.
            Gui.update (HeapsZipper.toList gameState.Heap)
            let! msg = q.Receive()
            match msg with
                | Move (a,b)    -> 
                    let newGameState = move (a,b) gameState
                    if gameState.AiEnabled then return! ready(newGameState)
                    else return! ready(newGameState)
                | _             -> failwith "Ready: Unexpected Message"
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
