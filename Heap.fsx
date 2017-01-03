type Heap =
  | Heap of int

// emptyHeap
let empty =
    Heap 0

// init Heap -- her må jeg godt have init fordi empty er lidt broken.
let init n =
    Heap n

// init Heap to int -- toString ?
let toInt (Heap n) =
    n

// add n to a Heap -- kan inline ?
let add n (Heap x) =
    Heap (x+n)

// subtract n from a Heap -- kan inline ?
let subtract n (Heap x) =
    Heap (x-n)

(*
printfn "%A" (empty = Heap 0)
printfn "%A" (init 3 = Heap 3)
printfn "%A" (init -3)
printfn "%A" (toInt (Heap 3) = 3)
printfn "%A" (add 3 (Heap 4) = (Heap 7))
printfn "%A" (subtract 3 (Heap 4) = (Heap 1))
printfn "%A" (subtract 5 (Heap 4))
*)
