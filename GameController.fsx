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
        Heap: HeapsZipper.HeapZipper;
        AiEnabled: bool
    }

    //let mutable gameState = {}

    let move (a,b) gs = { 
            TurnBit = (not gs.TurnBit);
            Heap =  (HeapsZipper.subtract a b gs.Heap);
            AiEnabled = gs.AiEnabled;
        }

    let setupNewGame (enableAI) = {
            TurnBit = if rand.Next(0, 1) = 1 then true else false;
            Heap = HeapsZipper.initFromList [for i in 1 .. rand.Next(3, 15) -> rand.Next(1, 100)];
            AiEnabled = enableAI;
        }

    let q = AsyncEventQueue.instance
    let rec menu() =
        async {
            
            let! msg = q.Receive()
            match msg with
                | NewGame   -> 
                    let ng = setupNewGame(false)
                    printfn "%A" (HeapsZipper.toList ng.Heap)
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
            
            // If focus = 0 then someone won.
            if Heap.toInt (HeapsZipper.focus gameState.Heap) = 0 then
                return! winGame(gameState)
            else

            Gui.update (HeapsZipper.toList gameState.Heap)
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
        }
    and winGame(gameState) =
        async {
            failwith "Not implemented"
        }