// for parsing Int active patterns
// denne skal ind i parsers sammen med den anden fra gui
let (|Int|_|) str =
    match System.Int32.TryParse(str) with
        | (true,int) -> Some(int)
        | _ -> None


let (|Win|_|) x =
    match HeapsZipper.focus x with
        | x when x = 0 ->
            Some(x)
        | _ -> None

