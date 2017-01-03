// der er noget galt med module systemet.
// i fsharp giver List.head en fejl i andre sprog bruger man maybes mere hvorfor???
// the most changable parameter should come last
// SPØRGSMÅL hvor tilladt er det at beytte failwith
// single case union type HVORFOR?
// til den her skal der tilføjes hvis den skal kunne tegnes men der er måske ikke nødvendigt
// da man jo i virkeligheden tager matches..
type Heap =
  | Heap of int 
  // kan måske bruge uint32 https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/primitive-types
  // så kan man ikke lave for lille heapinit
  // og med zipper kan man controllere valget


// empty matches
let empty = Heap 0

/// single Heap
//husk at ændre navn
let init n =
    Heap n

let toInt (Heap n) =
    n

//add n to a Heap
//skal måske også laves til ad one
let add n (Heap x) =
    Heap (x+n)

//subtract n from a Heap

//mulig ide til subtract.... man subtracter kun 1 ad gangeng så kan vi have en type
// type subtractable =
//    = isEmpty   -- denne skal måske ikke være der da vi ikke viser empties...
//    | isOneMatch
//    | isMoreMatches 2
//husk du tager altid 1 TING
//husk du tager altid 1 TING
//husk du tager altid 1 TING

// JEG MÅ IKKE KUNNE HEAPINIT med -5 fx

let subtract n (Heap x) =
    Heap (x-n)
