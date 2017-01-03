module Gui

    #load "AsyncEventQueue.fsx"
    open AsyncEventQueue
    open System.Windows.Forms
    open System.Drawing

    let mainWindow = new Form(Text="Nim Player",Size=Size(520,500))
    // Next line disables resizing of the window
    mainWindow.FormBorderStyle <- FormBorderStyle.FixedSingle

    // Add a panel, to group up all the radio buttons
    let heapPanel = new Panel(Size=Size(500,390),Top=10,Left=10)
    heapPanel.AutoScroll <- true
    heapPanel.BackColor <- Color.LightGray
    let radioBox = new GroupBox(Size=Size(500,390),Top=10,Left=10)

    // Add the buttons/text fields in the bottom of the screen.
    let newGameButton =
      new Button(Location = Point(10,410), Size=Size(100,22),Text = "New Game")

    let newAiGameButton =
      new Button(Location = Point(10,435), Size=Size(100,22),Text = "New AI Game")

    let makeDrawButton =
      new Button(Location = Point(380,420), MinimumSize=Size(100,50),
                 MaximumSize = Size(100,50),Text = "Draw!")

    let chooseMatches = 
      new TextBox(Location = Point(150,420),Size = Size(200,25))

    
    (* This list is keeping track of how many heaps are added for render, so
     * that we can correctly track the heap selected by the heap.
     *)
    let mutable selectedHeap        = -1
    let mutable selectedNumMatches  = -1
    (* Given a list of integers of size n,
     * creates n radiobuttons.
     * The integers in the list is used as label for each button.
     * 
     * fun type: int list -> unit
     *)
    let populateHeapPanel heaps =
        printfn "Populating panel with"
        let rec inner heaps yPos count = 
            match heaps with
            | []        -> ()
            | x::xs     ->
                let rad = new RadioButton(Text=x.ToString(),Top=yPos,Left=30)
                let radioSelectHandler event =
                    selectedHeap <- count
                    //AsyncEventQueue.instance.Post (AsyncEventQueue.SelectHeap count)

                rad.Click.Add(radioSelectHandler)
                heapPanel.Controls.Add rad
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
        clearHeapPanel()
        populateHeapPanel heaps


    // Add all the items.
    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add newAiGameButton
    mainWindow.Controls.Add chooseMatches
    mainWindow.Controls.Add makeDrawButton

    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     * 
     * fun type: System.EventArgs -> unit
     *)
    let newGameHandler _ = 
        AsyncEventQueue.instance.Post NewGame
    newGameButton.Click.Add(newGameHandler)


    let newAiGameHandler _ =
        AsyncEventQueue.instance.Post NewAiGame
    newAiGameButton.Click.Add(newAiGameHandler)

    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     * 
     * fun type: System.EventArgs -> unit
     *)
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
            | -1,_       -> AsyncEventQueue.instance.Post AsyncEventQueue.Error
            | _,-1       -> AsyncEventQueue.instance.Post AsyncEventQueue.Error
            | _,_        -> AsyncEventQueue.instance.Post (Move (selectedHeap, numMatches))

    makeDrawButton.Click.Add(makeDrawHandler)

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
        // Not sure if I should add event to queue here.
        // This will be more clear when we make the automaton I think.
        selectedNumMatches <- 
            try
                int chooseMatches.Text
            with
                | _ as d -> 
                    printfn "Could not parse integer: %s" chooseMatches.Text
                    -1        
    chooseMatches.TextChanged.Add(chooseMatchesHandler)

    
    

    mainWindow.Controls.Add heapPanel