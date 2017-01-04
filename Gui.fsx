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

let newGameHandler _ =
    q.Post AsyncEventQueue.NewGame


// kan måske argumentere for den her skal have argument med..
// så kan man også slå den sammen med newgame
(*
newAiGameButton.Click.Add(fun _ -> q.Post AsyncEventQueue.NewAiGame)
*)

let chooseMatchesHandler _ =
    q.Post (AsyncEventQueue.Write chooseMatches.Text)


let makeDrawHandler _ =
    q.Post (AsyncEventQueue.Take)


newGameButton.Click.Add(newGameHandler)
makeDrawButton.Click.Add(makeDrawHandler)

chooseMatches.TextChanged.Add(chooseMatchesHandler)


// this is bad compared to elm
// there should be a state on our model for window or this is very not functional
// kan stadig godt benyttes som en zipper men altså whats the point.
let drawHeapsFromList(xs) =
    let mutable y = 0
    for (x, i) in xs do

        let check = not(System.Convert.ToBoolean(i : int))

        let radioButton =
            new RadioButton(Checked=check,Text=x.ToString(),Top=y,Left=30)

        let radioSelectHandler _ =
            q.Post (AsyncEventQueue.SelectHeap i)

        radioButton.Click.Add(radioSelectHandler)
        heapsPanel.Controls.Add(radioButton)

        //mutable
        y <- y + 20

let clearHeapsZipper() =
    heapsPanel.Controls.Clear()


let setWrite(str) =
    chooseMatches.Text <- str
