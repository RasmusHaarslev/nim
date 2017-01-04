module Ai
    #load "AsyncEventQueue.fsx"
    #load "GameController.fsx"
    open AsyncEventQueue
    open GameController

    (* Recursively xorbs all elements in the list heap
     * m = a0 ^^^ a1 ^^^ ... ^^^ ax
     *
     * fun type: int list -> int
     *)
    let rec findm heap =
        match heap with
        | [] -> 0
        | x::xs -> x ^^^ findm xs

    (* Recursively traverses the list heap, and looks for a number, where when
     * xorb'ed with m, it returns a number smaller than the original number.
     * Returns a tuple of the index and the number.
     *
     * fun type: int list -> int -> int * int
     *)
    let findMove heap m =
        let rec innerFoo heap m idx =
            match heap with
            | [] -> failwith "Couldn't find k where ak < m" // Shouldn't ever happen
            | x::xs ->
                let ak = x ^^^ m
                in
                    if ak < x then (idx, x - ak) // Found right k
                    else innerFoo xs m (idx+1) // Try next k
        in
            innerFoo heap m 0

    (* Finds the index of the element in the list with the highest value.
     *)
    let maxIndexBy list =
        list
        |> List.mapi (fun i x -> i,  x)
        |> List.maxBy snd
        |> fst

    (* Finds the next play-move for the AI
     *)
    let aiMove gameState =
        let m = findm gameState.Heap

        let move =
            if m = 0 
            then (maxIndexBy gameState.Heap, 1)
            else (findMove gameState.Heap m)
        in
            match gameState.Difficulty with
            | GameController.Easy       -> move
            | GameController.Medium     -> move
            | GameController.Hard       -> move
            | GameController.Godlike    -> move
