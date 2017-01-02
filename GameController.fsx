module GameController
    
    #load "AsyncEventQueue.fsx"
    #load "Gui.fsx"

    open Gui
    open AsyncEventQueue

    let rand = System.Random()
    type GameState = {
        TurnBit: int;
        Heap: int list;
        SelectedHeap: int;
        RemoveCount: int
    }

    let mutable gameState = {
        TurnBit = 0;
        Heap = [1];
        SelectedHeap = 0;
        RemoveCount = 1
    }

    let rec ready() =
        async {
            Gui.populateHeapPanel gameState.Heap

            let! msg = AsyncEventQueue.ev.Receive()
            match msg with
                | Move (a,b)    -> printfn "Moved with: (%d,%d)" a b
                | SelectHeap a  -> printfn "Selected Heap: %d" a
                | Error         -> printfn "Error"
                | _             -> failwith "hallelujah"
        }

    //let newGameState =
    //    let gameState = {
    //        turnBit = rand.Next(0, 1);
    //        Heap = [for i in 1 .. rand.Next(1, 5) -> rand.Next(1, 100)];
    //        SelectedHeap = 0;
    //        RemoveCount = 1
    //    }
