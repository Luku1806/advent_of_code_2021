module part1

#load "../utils.fsx"

open System
open utils

let HeaderSize = 6

type Packet =
    {
        version: int
        packetType: int
        literal: int64 option
        subPackets: Packet list
        size: int
    }

let toHex (input: string) =
    input.ToCharArray()
    |> Array.map (fun char -> char.ToString())
    |> Array.map (fun char -> Convert.ToInt32(char, 16))
    |> Array.map (fun hex -> Convert.ToString(hex, 2))
    |> Array.map (fun binary -> binary.PadLeft(4, '0'))
    |> String.concat ""

let parsePacket (string: string) =
    let version = string.[0..2]
    let packetType = string.[3..5]
    let body = string.[6..]
    (Convert.ToInt32(version, 2), Convert.ToInt32(packetType, 2), body)

let isJustPadding (binaryData: string) =
    (not <| binaryData.Contains "1"
     && binaryData.Length <= 7)
    || binaryData.Length <= 7

let rec decode max (binaryData: string) (packets: Packet list) : Packet list =
    if max < 1
       || binaryData.Length < 8
       || isJustPadding binaryData then
        packets
    else
        let version, packetType, body = parsePacket binaryData

        let packet =
            match packetType with
            | 4 -> decodeLiteralPacket version body
            | _ -> decodeOperatorPacket version packetType body

        packet :: packets
        |> decode (max - 1) binaryData.[packet.size..]

and decodeLiteralPacket (version: int) (binaryData: string) =
    let literalBodyPairs =
        if binaryData.[0] = '0' then
            [| ([||], binaryData.[0..4].ToCharArray()) |]
        else
            binaryData.ToCharArray()
            |> Array.chunkBySize 5
            |> Array.pairwise
            |> Array.takeWhile (fun (before, _) -> before.[0] <> '0')

    let literalBody =
        literalBodyPairs
        |> Array.mapi
            (fun index (current, after) ->
                if index < literalBodyPairs.Length - 1 then current else Array.concat [ current; after ])
        |> Array.map String
        |> String.concat ""

    let binaryLiteral =
        literalBody.ToCharArray()
        |> Array.chunkBySize 5
        |> Array.map (fun chars -> String chars.[1..])
        |> String.concat ""

    {
        version = version
        packetType = 4
        literal = Some <| Convert.ToInt64(binaryLiteral, 2)
        subPackets = []
        size = HeaderSize + literalBody.Length
    }

and decodeOperatorPacket (version: int) (packetType: int) (binaryData: string) =
    if binaryData.[0] = '0' then
        let length = Convert.ToInt32(binaryData.[1..15], 2)

        {
            version = version
            packetType = packetType
            literal = None
            subPackets = decode Int32.MaxValue binaryData.[16..16 + length - 1] []
            size = HeaderSize + 1 + 15 + length
        }
    else
        let subPacketCount = Convert.ToInt32(binaryData.[1..11], 2)

        let subPackets = decode subPacketCount binaryData.[12..] []

        let subPacketsLength =
            subPackets
            |> List.sumBy (fun packet -> packet.size)

        {
            version = version
            packetType = packetType
            literal = None
            subPackets = subPackets
            size = HeaderSize + 1 + 11 + subPacketsLength
        }

let rec versionSum (packets: Packet list) =
    packets
    |> List.fold
        (fun sum packet ->
            sum
            + packet.version
            + versionSum packet.subPackets)
        0

let solve =
    file "input.txt"
    |> toHex
    |> (fun binary -> decode Int32.MaxValue binary [])
    |> tap (fun x -> printfn "%A" x)
    |> versionSum

solve
