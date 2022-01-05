(**
---
layout: standard
title: Composition
toc:
    to: 3
---
*)

(*** hide ***)

#I "../../../src/bin/Debug/netstandard2.0"
#r "Thoth.Json.dll"

open Thoth.Json

(**

By default, Thoth.Json provides support for most of the primitives types like `int`, `bool`, `System.Guid`, etc.

In order to build more complex types, it offers several ways to compose decoders.

## Object builder style

When working with objects, it is recommanded to use the object builder helper.

It has the benefit of putting the fields decoder and association at the same level. Indeed, when using
[map functions](#map-functions) it is easy to mess up the arguments order.

It also supports an record with any number of properties.

When using the object builder API, you first choose if the
property is required or optional.

Then you describe, for each property, how to decode it.

*)

type Point =
    {
        X : int
        Y : int
    }

Decode.object (fun get ->
    {
        X = get.Required.Raw (Decode.field "x" Decode.int)
        Y = get.Required.Raw (Decode.field "y" Decode.int)
    }
)

(**

Object builder also provides more friendly syntax
for the most common cases.

The decoder from above can be written as:

*)

Decode.object (fun get ->
    {
        X = get.Required.Field "x" Decode.int
        Y = get.Required.Field "y" Decode.int
    }
)

(**

## Map functions

The `map2`, `map3`, ..., `map8` functions take a function to build a concrete type from the result of the provided decoders.

Thoth.Json only provides `map` function up to 8 arguments, if you need more
consider using [Object builder](#object-builder) or implementing your own `mapX` function.

*)

Decode.map2 (fun x y ->
        {
            X = x
            Y = y
        }
    )
    (Decode.field "x" Decode.int)
    (Decode.field "y" Decode.int)

(**

## Chain decoders

You can use `andThen` in order to use the result of one decoder as the input of another one.

*)

type PersonType =
    | Student
    | Teacher

module PersonType =

    let decoder : Decoder<PersonType> =
        Decode.string
        |> Decode.andThen (fun textValue ->
            match textValue with
            | "student" ->
                Decode.succeed Student

            | "teacher" ->
                Decode.succeed Teacher

            | invalid ->
                Decode.fail $"""Expecting "student" or "teacher" but instead got: "%s{invalid}"""
        )

(**

The decoder will succeed only if the JSON value is a string and the string is "student" or "teacher".

## Map result to another type

When using DDD (aka Domain Driven Design) you often need to map your types.

*)

type Email = Email of string

module Email =

    let decoder : Decoder<Email> =
        Decode.string
        |> Decode.map Email

(**

If the provided JSON is a string, the decoder will succeed and return an Email type.

:::info
For simplicity, we used `Decode.string` but in a real world scenario, you would probably want to validate the email format.
:::

*)
