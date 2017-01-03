#load "Heaps.fsx"

open Heaps

(*
(*
type Status
    = isEmpty
    | isOneMatch
    | isMoreMatches 2
*)
// having zero Heaps is now impossible...
(*
type State =
    { previous : Heaps
    ; current : Heap
    ; remaining : Heaps
    ; chosenMatches : int
    }


type HeapsZipper =
    | HeapsZipper of State


let initialState =
    HeapsZipper
        { previous = Heaps.empty
        ; current = Heap.init 3 // må ikke kunne være heap.init 0
        ; remaining = Heaps.empty
        ; chosenMatches = 1 // må ikke kunne være 0
        }
*)

//printfn "%A" initialState

// init bør tage en state


// back
*)




type HeapZipper =
    | HeapZipper of Heaps.Heaps * Heap.Heap * Heaps.Heaps

//forward : heapZipper -> heapZipper
let forward = function
    | HeapZipper (prev, x, remaining) when not (Heaps.isEmpty remaining) ->
        HeapZipper (Heaps.cons prev x, Heaps.hd remaining, Heaps.tl remaining)
    | x -> x


let backward = function
    | HeapZipper (prev, x, remaining) when not (Heaps.isEmpty prev) ->
        HeapZipper (Heaps.tl prev, Heaps.hd prev, Heaps.cons remaining x)
    | x -> x


let initHeapZipper prev x remaining =
    HeapZipper (prev, x, remaining)

let initFromList xs =                                                                             
    HeapZipper (Heaps.empty,  Heap.init (List.head xs), Heaps.heapsFromListofInt (List.tail xs))

let rec goToBeginning xs =
    match xs with
        | HeapZipper (prev, x, remaining) when not (Heaps.isEmpty prev) ->
            goToBeginning (backward xs)
        | x -> x


let rec moveN n xs =
    if n > 0 then
      moveN (n-1) (forward xs)
    else
      xs


let focus (HeapZipper (_, x, _)) = x



// det lidt dum med gotobeginning.
let subtract i j xs =
    let (HeapZipper (prev, x, remaining)) = goToBeginning xs |> moveN i

    match Heap.toInt (Heap.subtract j x) with
        | i when i > 0 ->
            initHeapZipper prev (Heap.init i) remaining |> goToBeginning
        | i ->
            initHeapZipper prev (Heaps.hd remaining) (Heaps.tl remaining) |> goToBeginning



let toList (HeapZipper (prev, x, remaining)) =                                             
        Heaps.cons remaining x                                                  
                |> Heaps.merge prev 
                |> Heaps.toList


let myheapsses = Heaps.heapsFromListofHeap [Heap.init 4; Heap.init 3; Heap.init 2]






initHeapZipper Heaps.empty (Heap.init 1) myheapsses
  |> print
  |> moveN 2
  |> subtract 1 2
  |> print


(*
initHeapZipper empty (heapinit 1) myheapsses
  |> print
  |> moveN 2
  |> print
*)

(*
initHeapZipper empty (heapinit 1) myheapsses
  |> print
  |> forward
  |> print
  |> forward
  |> print
  |> forward
  |> print
  |> goToBeginning
  |> print
*)




(*
initHeapZipper empty (heapinit 1) myheapsses
  |> print
  |> forward
  |> print
  |> forward
  |> print
  |> backward
  |> print
  |> backward
  |> print
  |> backward
  |> print
*)
//heaps --> return all heaps
//currentHeap --> return current Heap








