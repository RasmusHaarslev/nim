module Gui

    open Types
    open AsyncEventQueue
    open System.Windows.Forms
    open System.Drawing

    let mainWindow = new Form(Text="Nim Player",Size=Size(290,500))
    // Next line disables resizing of the window
    mainWindow.FormBorderStyle <- FormBorderStyle.FixedSingle

    // Add a panel, to group up all the radio buttons
    let heapPanel = new Panel(Size=Size(270,390),Top=10,Left=10)
    heapPanel.AutoScroll <- true
    heapPanel.BackColor <- Color.LightGray
    let radioBox = new GroupBox(Size=Size(500,390),Top=10,Left=10)

    // Add the buttons/text fields in the bottom of the screen.
    let newGameButton =
      new Button(Location = Point(10,410), Size=Size(100,22),Text = "New Game")

    let newAiGameButton =
      new Button(Location = Point(10,435), Size=Size(100,22),Text = "New AI Game")

    let clearButton =
      new Button(Location = Point(10,410), Size=Size(100,47),Text = "Clear")

    let makeDrawButton =
      new Button(Location = Point(115,435), Size=Size(100,22),Text = "Draw!")

    let chooseMatches =
      new TextBox(Location = Point(115,410),Size = Size(165,22))

    let showSettingsButton =
      new Button(Location = Point(217,435), Size=Size(60,22),Text = "Options")

    let logWindow =
      new TextBox(Location = Point(295,10),Size = Size(205,200))
    logWindow.Multiline <- true
    logWindow.ScrollBars <- ScrollBars.Both
    // Shouldn't be editable.
    logWindow.ReadOnly <- true

    let urlLabel =
        new Label(Location = Point(300, 265), Size = Size(205, 15), Text = "Download from URL")

    let urlBox =
      new TextBox(Location = Point(295,280), Size = Size(205,150))
    urlBox.Multiline <- true

    let downloadButton =
        new Button(Location = Point(295, 435), Size = Size(205, 22), Text = "Download")

    (*
     * RadioButtons for changing difficulty.
     *)
    // Off is not relevant yet. Need more implementation work
    //let radioButtonDiffOff = new RadioButton()
    let radioButtonDiffEasy = new RadioButton(Location = Point(300,245), Text = "Easy")
    let radioButtonDiffMedium = new RadioButton(Location = Point(350,225), Text = "Medium")
    let radioButtonDiffHard = new RadioButton(Location = Point(300,225), Text = "Hard")
    let radioButtonDiffGodlike = new RadioButton(Location = Point(350,245), Text = "Godlike", Checked = true)
    let difficultyLabel =
        new Label(Location = Point(300, 215), Size = Size(205, 15), Text = "Choose AI Difficulty")
    (*
     * Add log message to logtextwindow
     * Fun type: string -> unit
     *)
    let addLogMessage msg =
        logWindow.Text <- (logWindow.Text + "\n" + msg)
        logWindow.SelectionStart <- logWindow.Text.Length
        logWindow.ScrollToCaret()

    (*
     * Enable/disable drawing.
     * Fun type: boolean -> unit
     *)
    let toggleDrawing b =
        printfn "Toggling drawing"
        makeDrawButton.Enabled <- b
        chooseMatches.ReadOnly <- not b

    let hideButtons bs =
        printfn "Disabling Buttons."
        for (b:Button) in bs do
            b.Enabled  <- false
            b.Visible  <- false

    let showButtons bs =
        printfn "Enabling Buttons"
        for (b:Button) in bs do
            b.Enabled  <- true
            b.Visible  <- true

    let setDiffEnabled b =
        radioButtonDiffEasy.Enabled <- b
        radioButtonDiffMedium.Enabled <- b
        radioButtonDiffHard.Enabled <- b
        radioButtonDiffGodlike.Enabled <- b

    (* These mutable variables are all used to keep track of state.
     * They were deemed too trivial or unsuitable to result in a node in our automaton.
     *)
    let mutable selectedHeap        = -1
    let mutable selectedNumMatches  = 1
    let mutable optionsShown        = false
    let mutable aiDiff              = Godlike

    (* Given a list of integers of size n,
     * creates n radiobuttons.
     * The integers in the list is used as label for each button.
     *
     * fun type: int list -> unit
     *)
    let populateHeapPanel heaps =
        printfn "Populating panel"
        let rec inner heaps yPos count =
            match heaps with
            | []        -> ()
            | x::xs     ->
                let rad = new RadioButton(Text=x.ToString(),Top=yPos,Left=30)
                let radioSelectHandler event =
                    selectedHeap <- count
                    toggleDrawing true
                    //AsyncEventQueue.instance.Post (AsyncEventQueue.SelectHeap count)

                rad.Click.Add(radioSelectHandler)
                heapPanel.Controls.Add rad
                //radioButtonList <- radioButtonList @ [rad]
                inner xs (yPos + 20) (count + 1)

        in
            inner heaps 0 0


    (* Clears all the radiobuttons from the heap panel
     *
     * fun type: unit -> unit
     *)
    let clearHeapPanel () =
        printfn "Clearing HeapPanel"
        while heapPanel.HasChildren do
            heapPanel.Controls.RemoveAt(0)
        ()

    let update heaps =
        chooseMatches.Text <- selectedNumMatches.ToString()
        clearHeapPanel()
        populateHeapPanel heaps


    // Add all the items.
    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add newAiGameButton
    mainWindow.Controls.Add clearButton
    mainWindow.Controls.Add chooseMatches
    mainWindow.Controls.Add makeDrawButton
    mainWindow.Controls.Add showSettingsButton
    mainWindow.Controls.Add logWindow
    mainWindow.Controls.Add urlLabel
    mainWindow.Controls.Add difficultyLabel
    mainWindow.Controls.Add urlBox
    mainWindow.Controls.Add downloadButton

    mainWindow.Controls.Add radioButtonDiffMedium
    mainWindow.Controls.Add radioButtonDiffGodlike
    mainWindow.Controls.Add radioButtonDiffEasy
    mainWindow.Controls.Add radioButtonDiffHard



    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     *
     * fun type: System.EventArgs -> unit
     *)
    let newGameHandler _ =
        AsyncEventQueue.instance.Post (NewGame NoAI)
    newGameButton.Click.Add(newGameHandler)


    let newAiGameHandler _ =
        AsyncEventQueue.instance.Post (NewGame aiDiff)
    newAiGameButton.Click.Add(newAiGameHandler)

    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     *
     * fun type: System.EventArgs -> unit
     *)
    let makeDrawHandler _ =
        printfn "Making draw with heap: %d, with %d matches." selectedHeap selectedNumMatches

        match selectedHeap,selectedNumMatches with
            | -1,_       -> AsyncEventQueue.instance.Post Error
            | _,-1       -> AsyncEventQueue.instance.Post Error
            | _,_        ->
                AsyncEventQueue.instance.Post (Move (selectedHeap, selectedNumMatches))
                toggleDrawing false
                selectedHeap <- 0


    makeDrawButton.Click.Add(makeDrawHandler)

    (* Handler function for toggle options.
     *
     * fun type: System.EventArgs -> unit
     *)
    let showOptionsHandler _ =
        printfn "Toggle Options"
        if optionsShown then
            mainWindow.Size <- Size(290,500)
        else
            mainWindow.Size <- Size(520,500)
        optionsShown <- not optionsShown
        ()

    let clearHandler _ =
        printfn "Clearing"
        AsyncEventQueue.instance.Post Clear
    clearButton.Click.Add clearHandler

    let downloadButtonHandler _ =
        //AsyncEventQueue.instance.Post (Download (urlBox.Text))
        ()
    downloadButton.Click.Add downloadButtonHandler


    showSettingsButton.Click.Add showOptionsHandler

    (* handlerFunction for the text field to choose matchNum
     * Error handling at this point is annoying. I suggest we don't!
     * If the users change can't be parsed as int, we set selected amount of
     * matches to -1, so that we raise an error later.
     *
     * MAYBE NO ERROR HANDLING SHOULD BE DONE IN THIS FILE? Need discussion.
     *
     * fun type: System.EventArgs -> unit
     *)
    let chooseMatchesHandler _ =
        selectedNumMatches <-
            try
                if int chooseMatches.Text = 0
                then
                    printfn "Choosing 0 matches is not allowed! Cheater!"
                    chooseMatches.Text <- "1"
                    1
                else
                    int chooseMatches.Text
            with
                | d ->
                    printfn "Could not parse integer: %s" chooseMatches.Text
                    chooseMatches.Text <- "1"
                    1
    chooseMatches.TextChanged.Add(chooseMatchesHandler)


    (* Handlers for changing difficulty.
     *)
    let easyDiffHandler _ =
        printfn "Easy Difficulty Enabled"
        aiDiff <- Easy
    radioButtonDiffEasy.Click.Add easyDiffHandler

    let mediumDiffHandler _ =
        printfn "Medium Difficulty Enabled"
        aiDiff <- Medium
    radioButtonDiffMedium.Click.Add mediumDiffHandler

    let hardDiffHandler _ =
        printfn "Hard Difficulty Enabled"
        aiDiff <- Hard
    radioButtonDiffHard.Click.Add hardDiffHandler

    let godlikeDiffHandler _ =
        printfn "Godlike Difficulty Enabled"
        aiDiff <- Godlike
    radioButtonDiffGodlike.Click.Add godlikeDiffHandler


    mainWindow.Controls.Add heapPanel
