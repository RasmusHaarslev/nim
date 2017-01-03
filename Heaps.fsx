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
let heapempty = 0

/// single Heap
//husk at ændre navn
let heapinit n =
    Heap n

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







type Heaps =
    | LinkedHeaps of Heap * Heaps
    | EmptyHeaps


// empty Heap
let empty = EmptyHeaps

// init
let init x = LinkedHeaps (x, EmptyHeaps)


// cons
let cons xs x =
    LinkedHeaps (x, xs)

let tl = function
    | LinkedHeaps (x, xs) -> xs
    | _ -> failwith "Cannot do tl, on empty element"


let hd = function
    | LinkedHeaps (x, xs) -> x
    | _ -> failwith "Cannot do hd, on empty element"


let isEmpty = function
    | EmptyHeaps -> true
    | _ -> false



// merge
let rec merge xs ys =
    match xs, ys with
        | EmptyHeaps, h | h, EmptyHeaps -> h
        | LinkedHeaps (h, tl), h2 ->
            LinkedHeaps (h, merge tl h2)


// index at position
let rec elementAt i xs =
    match xs, i with
        | LinkedHeaps (h, _), i when i < 1 -> h
        | LinkedHeaps (_, tl), i -> elementAt (i-1) tl
        // SPØRGSMÅL hvor tilladt er det at beytte failwith
        | _, _ -> failwith "Index out of bounds"



(*
// take elements n from a heap
let rec take i xs =
    match xs, i with
        | _, i when i < 1 -> EmptyHeaps
        | EmptyHeaps, _ -> EmptyHeaps
        | LinkedHeaps (h, tl), i ->
            LinkedHeaps (h, take (i-1) tl)


// drop elements n of a heap
let rec drop i xs =
    match xs, i with
        | xs, i when i < 1 -> xs
        | EmptyHeaps, _ -> EmptyHeaps
        | LinkedHeaps (h, tl), i ->
            drop (i-1) tl


// returns before, element, and after ((linkedHeaps, heap, linkeadheaps))
let splitOnIndex i xs =

    let before =
        take i xs

    let focus =
        elementAt i xs

    let after =
        drop (i+1) xs

    (before, focus, after)

*)



// DET JO EGENTLIGT DUMT AT FJERNE DEM for hvis vi gør det så kan vi jo ik tegne det?? men måske ikke problem ?? tilfø til zipper..
// DET JO EGENTLIGT DUMT AT FJERNE DEM for hvis vi gør det 
// DET JO EGENTLIGT DUMT AT FJERNE DEM for hvis vi gør det 
// DET JO EGENTLIGT DUMT AT FJERNE DEM for hvis vi gør det
// subtract some value n at index i from heaps xs
(*
let rec subtractFromNth i n xs =
    match xs, i with
        | LinkedHeaps (hd, tl), i when i < 1 ->
          // paspå der ikke bliver subtractet for meget
            LinkedHeaps (subtract n hd, tl)
        | LinkedHeaps (h, tl), i ->
            // skal vi sige du må ikke suprtracte der her tal eller skal vi bruge en slider fx og så kan man ikke gøre det???
            LinkedHeaps (h, subtractFromNth (i-1) n tl)
        | _, _ -> failwith "Index out of bounds"
*)

//BØR NOK HAVE NOGET I DEN HER STIL
// hvor subract er fra current datum..
(*
let rec subtractHeaps i = function
    | LinkedHeaps (
    | _, _ -> failwith "Heaps Is empty"
*)


// find maxheap

// find mværdi


// FLYT TIL EGET LIBRARy later
//let flip a b = b a


//lidt helpers der kan være brugbare.
let heapsFromListofHeap xs =
    List.fold cons EmptyHeaps xs




let print xs =
  printfn "%A" xs
  xs


(*
empty
    |> merge (init (heapinit 3))
    |> merge (init (heapinit 1))
    |> merge (init (heapinit 2))
    |> print
    |> subtractFromNth 1 2
    |> print
*)






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
    | HeapZipper of Heaps * Heap * Heaps

//forward : heapZipper -> heapZipper
let forward = function
    | HeapZipper (prev, x, remaining) when not (isEmpty remaining) ->
        HeapZipper (cons prev x, hd remaining, tl remaining)
    | x -> x


let backward = function
    | HeapZipper (prev, x, remaining) when not (isEmpty prev) ->
        HeapZipper (tl prev, hd prev, cons remaining x)
    | x -> x


let initHeapZipper prev x remaining =
    HeapZipper (prev, x, remaining)


let rec goToBeginning xs =
    match xs with
        | HeapZipper (prev, x, remaining) when not (isEmpty prev) ->
            goToBeginning (backward xs)
        | x -> x


let rec moveN n xs =
    if n > 0 then
      moveN (n-1) (forward xs)
    else
      xs


let focus (HeapZipper (_, x, _)) = x



//ændrenavn
let subtractIJ i j xs =
    let (HeapZipper (prev, x, remaining)) = goToBeginning xs |> moveN i

    initHeapZipper prev (subtract j x) remaining




let myheapsses = heapsFromListofHeap [heapinit 4; heapinit 3; heapinit 2]



initHeapZipper empty (heapinit 1) myheapsses
  |> print
  |> moveN 2
  |> subtractIJ 1 2
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


print "hey"


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








