#load "Heaps.fs"
#load "Heap.fs"

open Heaps
open Heap

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

type HeapZipper =
    | HeapZipper of Heaps * Heap * Heaps

//forward : heapZipper -> heapZipper
let forward = function
    // jeg må nok ikke skrive LinkedHeaps her
    | HeapZipper (prev, x, LinkedHeaps (hd, tl)) ->
        printfn "%A" prev
        HeapZipper (prev, Heap.init 2, tl)
    | x -> x


*)

let myheapsses = Heaps.cons Heaps.empty (Heap.init 3)

printfn "%A" myheapsses



//HeapZipper (Heaps.empty, Heap.init 3, myheapsses)
  //|> forward 
  //|> printfn "%A"

//heaps --> return all heaps
//currentHeap --> return current Heap
