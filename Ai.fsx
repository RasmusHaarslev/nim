#load "HeapsZipper.fsx"

open HeapsZipper

type Ai =
    | ImprovedAI of int

let initImprovedAI n = ImprovedAI n

let move heapsZipper (ImprovedAI n) =
    /// disse er stadig invalid moves da de skal checkes mod zipperen
    /// disse er stadig invalid moves da de skal checkes mod zipperen
    /// disse er stadig invalid moves da de skal checkes mod zipperen
    (2, 1)
        //let m = HeapsZipper.findM heapsZipper
        //if m = 0 then

        //else

      (*let m = Hea heap

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
      
//let findMove heapsZipper m =    


(*
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
