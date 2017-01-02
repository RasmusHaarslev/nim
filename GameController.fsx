module GameController

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

    //let newGameState =
    //    let gameState = {
    //        turnBit = rand.Next(0, 1);
    //        Heap = [for i in 1 .. rand.Next(1, 5) -> rand.Next(1, 100)];
    //        SelectedHeap = 0;
    //        RemoveCount = 1
    //    }
