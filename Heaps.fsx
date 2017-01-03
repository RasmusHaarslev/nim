#load "Heap.fsx"


open Heap

type Heaps =
    | LinkedHeaps of Heap.Heap * Heaps
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


    // cons                                                                                           
let consInt xs x =                                                          
    LinkedHeaps (Heap.init x, xs)                                           
                                                                                
let heapsFromListofInt xs =                                                 
    List.fold consInt EmptyHeaps xs                                         
                                   

let rec toList xs =                                                                               
        match xs with                                                           
                | EmptyHeaps -> []                                                  
                | LinkedHeaps (h, tl) ->                                            
                         h :: (toList tl)  

let print xs =
  printfn "%A" xs
  xs



empty
    |> merge (init (Heap.init 3))
    |> merge (init (Heap.init 1))
    |> merge (init (Heap.init 2))
    |> print






