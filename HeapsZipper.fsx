#load "Heaps.fsx"
open Heaps


type HeapsZipper =
    | HeapsZipper of Heaps.Heaps * Heap.Heap * Heaps.Heaps


// move zipper forward
let forward = function
    | HeapsZipper (prev, x, remaining) when not (Heaps.isEmpty remaining) ->
        HeapsZipper
            ( Heaps.cons x prev
            , Heaps.hd remaining
            , Heaps.tl remaining
            )
    | x -> x


// move zipper backward
let backward = function
    | HeapsZipper (prev, x, remaining) when not (Heaps.isEmpty prev) ->
        HeapsZipper
            ( Heaps.tl prev
            , Heaps.hd prev
            , Heaps.cons x remaining
            )
    | x -> x


// init Heapszipper
let init x =
    HeapsZipper (Heaps.empty, x, Heaps.empty)


// get HeapsZipper focus
let focus (HeapsZipper (_, x, _)) =  x


// insert Heap on front of Heapszipper
let insert y (HeapsZipper (prev, x, remaining)) =
    HeapsZipper (Heaps.cons y prev, x, remaining)


// this is onlyneeded coz no funtional view
// HeapsZipper toList of int
let toList (HeapsZipper (prev, x, remaining)) =
        Heaps.cons x remaining
            |> Heaps.merge prev
            |> Heaps.toList


// initialModel for testing purposes
let initialModel =
    init (Heap.init 1)
        |> insert (Heap.init 2)
        |> insert (Heap.init 3)
        |> insert (Heap.init 4)



//fix tomorrow
let subtract i xs =
    let (HeapsZipper (prev, x, remaining)) = xs
    HeapsZipper (prev, Heap.subtract i x, remaining)

(*
init (Heap.init 1)
    |> insert (Heap.init 2)
    |> insert (Heap.init 3)
    |> backward
    |> backward
    |> backward
    |> forward
    |> forward
    |> toList
    |> printfn "%A"
*)


// NOT happy with these shit functions below....
// det lidt dum med gotobeginning.
(*
let rec moveN n xs =
    if n > 0 then
        moveN (n-1) (forward xs)
    else
        xs

let rec goToBeginning xs =
    match xs with
        | HeapZipper (prev, x, remaining) when not (Heaps.isEmpty prev) ->
            goToBeginning (backward xs)
        | x -> x


let subtract i j xs =
    let (HeapZipper (prev, x, remaining)) = goToBeginning xs |> moveN i

    match Heap.toInt (Heap.subtract j x) with
        | i when i > 0 ->
              init prev (Heap.init i) remaining |> goToBeginning
        | i ->
              let heaps = Heaps.merge (Heaps.reverse prev) remaining
              if Heaps.isEmpty heaps then
                  init (Heap.init 0) Heaps.empty
              else
                  init (Heaps.hd heaps) (Heaps.tl heaps)
*)
