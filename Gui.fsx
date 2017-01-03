#load "AsyncEventQueue.fsx"
open AsyncEventQueue

open System.Windows.Forms
open System.Drawing

let q = AsyncEventQueue.instance

let mainWindow =
    new Form(Text="Nim Player",Size=Size(520,500))

let newGameButton =
    new Button(Location=Point(10,410),Size=Size(100,22),Text="New Game")

(*
let newAiGameButton =
    new Button(Location=Point(10,435),Size=Size(100,22),Text = "New AI Game")
*)

let heapsPanel =
    new Panel(Size=Size(500,390),Top=10,Left=10)


let makeDrawButton =
    new Button(Location = Point(380,420), MinimumSize=Size(100,50),
            MaximumSize = Size(100,50),Text = "Draw!")

let chooseMatches =
    new TextBox(Location = Point(150,420),Size = Size(200,25))

mainWindow.Controls.Add(newGameButton)
//mainWindow.Controls.Add(newAiGameButton)
mainWindow.Controls.Add(heapsPanel)
mainWindow.Controls.Add(makeDrawButton)
mainWindow.Controls.Add(chooseMatches)

newGameButton.Click.Add(fun _ -> q.Post AsyncEventQueue.NewGame)

// kan måske argumentere for den her skal have argument med..
// så kan man også slå den sammen med newgame
(*
newAiGameButton.Click.Add(fun _ -> q.Post AsyncEventQueue.NewAiGame)
*)




//VI SKAL HAVE MANGE FLERE STATES
// HVERGANG DU SKRIVER NOGET SKAL DER KOMME EN MESSAGE INDHOLDENDEDET det der står i textfetl
// så opdatere man textfelt..
// og så vurdere man new states..

let chooseMatchesHandler _ =
    selectedNumMatches <- 
        try
            int chooseMatches.Text
        with
            | _ as d -> 
                printfn "Could not parse integer: %s" chooseMatches.Text
                -1        

chooseMatches.TextChanged.Add(chooseMatchesHandler)











// this is bad compared to elm
// there should be a state on our model for window or this is very not functional
// kan stadig godt benyttes som en zipper men altså whats the point.
let drawHeapsFromList(xs) =
    let mutable y = 0
    let mutable count = 0
    for x in xs do

        let radioButton =
            new RadioButton(Text=x.ToString(),Top=y,Left=30)

        //kan godt move zipper her
        (*let radioSelectHandler _ =
            q.Post (AsyncEventQueue.SelectHeap count)

        radioButton.Click.Add(radioSelectHandler)
        *)
        heapsPanel.Controls.Add(radioButton)

        y <- y + 20
        count <- count + 1


let clearHeapsZipper() =
    heapsPanel.Controls.Clear()


(*i
let makeDrawHandler _ =
    let numMatches = 
        try
            int chooseMatches.Text
        with
            | _ as d -> 
                printfn "Could not parse integer: %s" chooseMatches.Text
                -1

    printfn "Making draw with heap: %d, with %d matches." selectedHeap selectedNumMatches

    match selectedHeap,numMatches with
        | -1,_       -> GameController.ev.Post GameController.Error
        | _,-1       -> GameController.ev.Post GameController.Error
        | _,_        -> GameController.ev.Post (GameController.Move (selectedHeap, numMatches))

*)

//makeDrawButton.Click.Add(makeDrawHandler)




(* handlerFunction for the text field to choose matchNum
 * Error handling at this point is annoying. I suggest we don't!
 * If the users change can't be parsed as int, we set selected amount of 
 * matches to -1, so that we raise an error later.
 * 
 * MAYBE NO ERROR HANDLING SHOULD BE DONE IN THIS FILE? Need discussion.
 *
 * fun type: System.EventArgs -> unit
 *)
