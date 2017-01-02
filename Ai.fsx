module Ai
    #load "AsyncEventQueue.fsx"
    open AsyncEventQueue

    // 
    let rec findm heap =
        match heap with
        | [] -> 0
        | x::xs -> x ^^^ findm xs

    let findMove heap m =
        let rec innerFoo heap m idx =
            match heap with
            | [] -> failwith "Couldn't find k where ak < m"
            | x::xs ->
                let ak = x ^^^ m
                in
                    if ak < x then (idx, m - ak)
                    else innerFoo xs m (idx+1)
        in
            innerFoo heap m 0

    let maxIndexBy list =
        list
        |> List.mapi (fun i x -> i,  x)
        |> List.maxBy snd
        |> fst

    let aiMove heap =
        let m = findm heap

        if m = 0 then
            let largestHeapIdx = maxIndexBy heap
            in
                printf "Apply move (%d, 1)" largestHeapIdx
        else
            let (heapIdx, removeCount) = findMove heap m
            in
                printf "Apply move (%d, %d)" heapIdx removeCount
