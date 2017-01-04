#load "Heap.fsx"
open Heap


type Heaps =
    | LinkedHeaps of Heap.Heap * Heaps
    | EmptyHeaps


// empty Heaps
let empty =
    EmptyHeaps


// init with a Heap
let init x =
    LinkedHeaps (x, empty)


// cons Heaps and Heap
let cons x xs =
    LinkedHeaps (x, xs)


// tail of Heaps -- unsafe
let tl = function
    | LinkedHeaps (x, xs) -> xs
    | _ -> failwith "Cannot do tl, on empty element"


// head of Heaps -- unsafe
let hd = function
    | LinkedHeaps (x, xs) -> x
    | _ -> failwith "Cannot do hd, on empty element"


// isEmpty Heaps ?
let isEmpty = function
    | EmptyHeaps -> true
    | _ -> false

// find m for heaps
let rec findM = function
    | EmptyHeaps -> 0
    | LinkedHeaps (x, xs) -> (Heap.toInt x) + (findM xs)


// reverse heaps
let reverse xs =
    let rec aux acc = function
          | EmptyHeaps -> acc
          | LinkedHeaps (y, ys) ->
              aux (cons y acc) ys
    aux empty xs


// merge two Heaps
let rec merge xs ys =
    match xs, ys with
        | EmptyHeaps, h | h, EmptyHeaps -> h
        | LinkedHeaps (h, tl), h2 ->
            LinkedHeaps (h, merge tl h2)

// this is onlyneeded coz no funtional view
// Heaps to list of ints
let toIndexedList xs =
    let rec aux i = function
        | EmptyHeaps -> []
        | LinkedHeaps (h, tl) ->
            (Heap.toInt h, i) :: (aux (i+1) tl)
    aux 1 xs


//find maxheap
//find mværdi

(*
printfn "%A" (empty = EmptyHeaps)
printfn "%A" (init (Heap.init 3) = (LinkedHeaps ((Heap.init 3), EmptyHeaps)))
empty
    |> cons (Heap.init 3)
    |> cons (Heap.init 2)
    |> cons (Heap.init 1)
    |> printfn "%A"
printfn "%A" (tl ( empty |> cons (Heap.init 3)) = empty)
printfn "%A" (tl ( empty |> cons (Heap.init 3) |> cons (Heap.init 2)))
printfn "%A" (hd ( empty |> cons (Heap.init 3)) = (Heap.init 3))
printfn "%A" (isEmpty empty)
printfn "%A" (isEmpty (empty |> cons (Heap.init 3)) = false)
printfn "%A" (reverse (empty |> cons (Heap.init 3) |> cons (Heap.init 2)))
printfn "%A" (reverse (empty |> cons (Heap.init 3) |> cons (Heap.init 2)))
printfn "%A" (merge (empty |> cons (Heap.init 4))
    (empty |> cons (Heap.init 3) |> cons (Heap.init 2)))
printfn "%A" (toList (empty |> cons (Heap.init 3) |> cons (Heap.init 2)))
*)
