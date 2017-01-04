#load "AsyncEventQueue.fsx"
open AsyncEventQueue

open System.Windows.Forms
open System.Drawing

let q = AsyncEventQueue.instance

let mainWindow =
    new Form(Text="Nim Player",Size=Size(720,700))

let newGameButton =
    new Button(Location=Point(10,410),Size=Size(100,22),Text="New Game")

let newAiGameButton =
    new Button(Location=Point(10,435),Size=Size(100,22),Text="New AI Game")

let menuButton =
    new Button(Location=Point(10,410),Size=Size(100,22),Text="Go To Menu")

let heapsPanel =
    new Panel(Size=Size(500,390),Top=10,Left=10)

let logPanel =
    new Panel(Size=Size(200,200),Top=480,Left=10)

let choosePanel =
    new Panel(Size=Size(200,200),Top=420,Left=140)

let makeDrawButton =
    new Button(Location=Point(380,420),MinimumSize=Size(100,50),
            MaximumSize=Size(100,50),Text="Draw!")



mainWindow.Controls.Add(newGameButton)
mainWindow.Controls.Add(newAiGameButton)
mainWindow.Controls.Add(menuButton)
mainWindow.Controls.Add(heapsPanel)
mainWindow.Controls.Add(logPanel)
mainWindow.Controls.Add(makeDrawButton)
mainWindow.Controls.Add(choosePanel)


let newGameHandler _ =
    q.Post AsyncEventQueue.NewGame

let newAiGameHandler _ =
    q.Post AsyncEventQueue.NewAiGame

let menuHandler _ =
    q.Post AsyncEventQueue.Menu


let makeDrawHandler _ =
    q.Post (AsyncEventQueue.Take)

newGameButton.Click.Add(newGameHandler)
newAiGameButton.Click.Add(newAiGameHandler)
menuButton.Click.Add(menuHandler)
makeDrawButton.Click.Add(makeDrawHandler)


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

let initLog(str) =

    let logWindow =
        new TextBox(ReadOnly=true,Text=str,Size=Size(200,25))

    logPanel.Controls.Add(logWindow)


let clearLog() =
    logPanel.Controls.Clear()


let initChooseMatches(str) =

    let chooseMatches =
        new TextBox(Text=str,Size=Size(200,25))

    let chooseMatchesHandler _ =
        q.Post (AsyncEventQueue.Write chooseMatches.Text)

    chooseMatches.TextChanged.Add(chooseMatchesHandler)

    choosePanel.Controls.Add(chooseMatches)


let clearChooseMatches() =
    choosePanel.Controls.Clear()


let clearHeapsZipper() =
    heapsPanel.Controls.Clear()


let showNewGameButton(b) =
    newGameButton.Visible <- b

let showMenu(b) =
    menuButton.Visible <- b

let showNewAiGameButton(b) =
    newAiGameButton.Visible <- b

let showDrawButton(b) =
    makeDrawButton.Visible <- b
