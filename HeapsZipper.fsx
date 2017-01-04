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


// insert Heap on front of Heapszipper
let insert y (HeapsZipper (prev, x, remaining)) =
    HeapsZipper (Heaps.cons y prev, x, remaining)


// initialModel for testing purposes
let initialModel =
    init (Heap.init 1)
        |> insert (Heap.init 4)
        |> insert (Heap.init 6)
        |> insert (Heap.init 9)




//fix tomorrow these tomorrow
//fix tomorrow these tomorrow
//fix tomorrow these tomorrow
// this is onlyneeded coz no funtional view
// HeapsZipper toList of int
let toIndexedList xs =
    let rec aux i = function
        | (HeapsZipper (prev, x, remaining)) when Heaps.isEmpty prev ->
            (Heap.toInt x, 0) :: (Heaps.toIndexedList remaining)

        | (HeapsZipper ( prev, x, remaining)) ->
            (Heap.toInt (Heaps.hd prev), i) ::
                (aux (i-1) (HeapsZipper (Heaps.tl prev, x, remaining)))

    //yeahhhh!
    List.sortBy (fun (_, y) -> -y) (aux (-1) xs)


// get HeapsZipper focus as int
let focus (HeapsZipper (_, x, _)) =
    Heap.toInt x



// move curser
let rec move n xs =
    match n with
      | n when n > 0 ->
          move (n-1) (forward xs)
      | n when n < 0 ->
          move (n+1) (backward xs)
      | n when n = 0 ->
          xs
      // wtf
      | _ ->
          failwith "n is not integer ?"


// someSubtract
//  brug active patterns?
let subtract n (HeapsZipper (prev, x, remaining)) =
    match Heap.toInt (Heap.subtract n x) with
        | i when i > 0 ->
            HeapsZipper (prev, Heap.init i, remaining)
        | i ->
              let heaps = Heaps.merge (Heaps.reverse prev) remaining

              if Heaps.isEmpty heaps then
                  HeapsZipper (Heaps.empty, Heap.init 0, Heaps.empty)
              else
                  HeapsZipper (Heaps.empty, Heaps.hd heaps, Heaps.tl heaps)


// someSubtract
//  brug active patterns?
let subtract2 n m heapsZipper =
    move n heapsZipper
        |> subtract m

(*
init (Heap.init -2)
    |> insert (Heap.init 2)
    |> insert (Heap.init 3)
    |> insert (Heap.init 4)
    |> subtract2 -2 1
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
