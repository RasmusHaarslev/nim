#load "AsyncEventQueue.fsx"
#load "Ai.fsx"
#load "Gui.fsx"
open System
open System.Windows.Forms
open System.Drawing

open AsyncEventQueue
open Ai
open Gui

Application.Run(Gui.mainWindow);
