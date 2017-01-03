module Heap

// single case union type
// til den her skal der tilføjes hvis den skal kunne tegnes men der er måske ikke nødvendigt
// da man jo i virkeligheden tager matches..
// til den her skal
// til den her skal




type Heap =
  | T of int

// empty matches
let empty = 0

/// single Heap
let init n =
    T n

//add n to a Heap
let add n (T x) =
    T (x+n)

//subtract n from a Heap
let subtract n (T x) =
    T (x-n)
