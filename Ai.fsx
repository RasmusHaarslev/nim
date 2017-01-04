type Ai =
    | SimpleAI
    | ImprovedAI

let initSimpleAI = SimpleAI

let initImprovedAI = ImprovedAI

let move heapsZipper = function
    | SimpleAI ->
        (1, 1)
    | ImprovedAI ->
        (0, 3)

(*
let rec findm heap =
        match heap with
        | [] -> 0
        | x::xs -> x ^^^ findm xs


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
    let aiMove heap =
        let m = findm heap

        if m = 0 then
            let largestHeapIdx: int = maxIndexBy heap
            in
                (largestHeapIdx, 1)
        else
            let (heapIdx, removeCount) = findMove heap m
            in
                (heapIdx, removeCount)
                *)
