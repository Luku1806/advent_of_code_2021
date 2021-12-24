#load "../utils.fsx"

open utils

open System

type Image = char array array

let Padding = 2

let dimensions (image: Image) = (image.[0].Length, image.Length)

let blankImage width height : Image =
    [|
        for y in 0 .. height - 1 -> [| for x in 0 .. width - 1 -> '.' |]
    |]

let addPadding (image: Image) (enhancements: int) =
    let padding = enhancements * Padding
    let totalPadding = 2 * padding

    let width, height = dimensions image

    blankImage (width + totalPadding) (height + totalPadding)
    |> Array.mapi
        (fun rowIndex row ->
            row
            |> Array.mapi
                (fun columnIndex pixel ->
                    let oldRow =
                        defaultArg (Array.tryItem (rowIndex - padding) image) row

                    defaultArg (Array.tryItem (columnIndex - padding) oldRow) pixel))

let toAlgorithmIndex (image: Image) (rowIndex: int) (columnIndex: int) =
    let square =
        [|
            (rowIndex - 1, columnIndex - 1)
            (rowIndex - 1, columnIndex)
            (rowIndex - 1, columnIndex + 1)
            (rowIndex, columnIndex - 1)
            (rowIndex, columnIndex)
            (rowIndex, columnIndex + 1)
            (rowIndex + 1, columnIndex - 1)
            (rowIndex + 1, columnIndex)
            (rowIndex + 1, columnIndex + 1)
        |]
        |> Array.map
            (fun (y, x) ->
                let row = defaultArg (Array.tryItem y image) [||]
                let pixel = defaultArg (Array.tryItem x row) '.'
                pixel)

    let index =
        square
        |> Array.fold
            (fun line pixel ->
                match pixel with
                | '#' -> line + "1"
                | _ -> line + "0")
            ""

    Convert.ToInt32(index, 2)


let enhanceImage (algorithm: string) (image: Image) =
    let width, height = dimensions image

    [| 1 .. height - Padding |]
    |> Array.map
        (fun rowIndex ->
            [| 1 .. width - Padding |]
            |> Array.map (fun columnIndex -> algorithm.[toAlgorithmIndex image rowIndex columnIndex]))

let applyAlgorithm (algorithm: string) (degree: int) (image: Image) =
    [| 1 .. degree |]
    |> Array.fold (fun image _ -> enhanceImage algorithm image) (addPadding image degree)


let draw (image: Image) =
    image
    |> Array.iter
        (fun row ->
            row |> Array.iter (printf "%c")
            printfn "")


let amountOfLitPixels (image: Image) =
    image
    |> flatten
    |> Array.filter (fun char -> char = '#')
    |> Array.length

let solve () =
    let algorithm = file "algorithm.txt"

    file "image.txt"
    |> toRows
    |> Array.ofList
    |> Array.map (fun line -> line.ToCharArray())
    |> tap draw
    |> applyAlgorithm algorithm 2
    |> tap draw
    |> tap (fun image -> printfn "Part 1: %d" (amountOfLitPixels image))
    |> applyAlgorithm algorithm 48
    |> tap draw
    |> tap (fun image -> printfn "Part 2: %d" (amountOfLitPixels image))


solve ()
