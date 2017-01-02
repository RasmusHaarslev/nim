let rec findm heap =
    match heap with
    | [] -> 0
    | [x] -> x
    | x::xs -> x ^^^ findm xs
