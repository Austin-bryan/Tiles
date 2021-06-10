﻿using System;
using System.Linq;
using UnityEngine;


public partial class LevelManager : MonoBehaviour
{
    public static string[] Levels = new string[]
    {
        "Test Level",
        "3, 6, Move(10)    | r-b-b-;",
        "5, 6, Move(20)    | pi- b- r- r- g/g{ic}/g-;",
        "6, 6, Move(30)    | y- b/x{l(1), wa(2)}/b/b/b/z{l(2), wa(1)}/ p- p/x{l(1), wa(1)}/p/p/p/z{l(2), wa(2)}/ b- y- ;",
        "5, 6, Move(20)    | y- b- p- p/<x{l(1)}, z{l(1)}>/p/p/<x{l(1)}, z{l(1)}>/ b- y- ;",
        "5, 6, Move(30)    | pi- b- r- r- g/g{d, ic, ba, ir(6)}/g-;",
        "5, 6, Move(20)    | pi{ba}/pi- b{ba}/b- r{ba}/r- r- g/g{d, ba}/g-;",
        "5, 6, Move(12)    | pi- b- r/r- r- g/g{d, ba}/g-;",
        "5, 6, Move(10)    | pi- b- r{ba}/r- r- g-;",
        "7, 6, Move(10)    | pi- b- r- y- y- r- g/g{d, ba}/g/g-;",
        "5, 6, Move(10)    | pi- b/b/b/b/b/ g/g/g/g/g/ r- y-;",
        "5, 6, Move(30)    | b/b/b{ro(9, cc)}/<b{wa(1), am(5)}, r>/b/ g/g{wa(1)}/g/g/g/ pi- r- y-;",
        "5, 6, Move(30)    | pi- b/b/b{ro(9, cc)}/b{st(5)}/b/ g/g{ba}/g/g/g/ r- y-;",
        "5, 6, Move(30)    | <r, z>/r/r/r/r/ b/<b, x>/b/b/b/ y/y/<y, y{n}>/y/y/ g/g/g/<g, g{ir(5)}>/g/ p/p/p/p/p;",
        "5, 6, Move(30)    | r/r{n}/r{s}/r{bo}/r/ b/b{n, s}/b{n, bo}/b{s, bo}/b/ y/y{n, bo, s}/y/y/y/ g/g/g/g/g/ p/p/p/p/p;",
        "5, 6, Move(30)    | r- b{wa(1), am(3)}/b- g/g{ro(9, cw)}/g{wa(1)}/g- b- r-;",
        "5, 6, Move(30)    | r- b{wa(1), st(3)}/b- g/g/g{wa(1)}/g- b- r-;",
        "3, 6, Move(30)    | r- g/g{ro(4, cw), ba}/g- b-;",
        "9, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b- r- y- pi- m- c-;",
        "6, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b- r- y-;",
        "4, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b-;",
        "5, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b- r-;",
        "7, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b- r- y- pi-;",
        "8, 6, Move(30)    | r- b- g/g{ro(4, cw), ba}/g- b- r- y- pi- m-",
        "5, 6, Move(30)    | r- b- g/g/g{ro(9, cw)}/g/g- b- r-;",
        "5, 6, Move(30)    | r- b- g/g/g{ro(2, cc, mini)}/g{ro(2, cc, mini)}/g- b- r-;",
        "5, 6, Move(30)    | r- b- g/g/g{ro(1, cc), st(5)}/g- b- r-;",
        "5, 6, Move(30)    | r- b- g/g/g{ro(8, cc), st(5)}/g- b- r-;",
        "5, 6, Move(30)    | r- b- g/g/g{ro(5, cc), st(5)}/g- b- r-;",
        "5, 6, Move(30)    | r- b- g/g/g{ro(9, cc), st(5)}/g- b- r-;",
        "5, 6, Move(30)    | x/r- b/x/b- g/x/g{ir(10)}/z/g/ b/z/b- z-;",
        "9, 6, Move(30)    | r*8/r{ic}/ o*7/o{ic}- y*6/y{ic}- g*5/g{ic}- c*4/c{ic}- b*3/b{ic}- p*2/p{ic}- pi/pi{ic}- m{ic}-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b*5/b{ba, wa(2)}/b/b/ g*4/g{ic, wa(2)}/g/g/g/ pi-;",
        "5, 6, Move(30)    | r- y- b/b/b{ic, ba}/b/b/ g- pi-;",
        "3, 6, Move(30)    | r- b/b{ic, ir(5)}/b/ g-;",
        "7, 6, Move(30)    | r-  b/b{ba}/b- b/b{ba}/b- b/b{ba}/b- b/b{ba}/b- b/b{ba}/b- g-;",
        "3, 6, Move(30)    | r- b/b{ir(5)}/b/ g-;",
        "3, 6, Move(30)    | r- b/b{ba, ir(5)}/b/ g-;",
        "3, 6, Move(30)    | r- b/b{ca, ir(5)}/b/ g-;",
        "5, 6, Move(30)    | z- z/r/g/b/z/ z/r{ic}/g/b/z/ z/r{ic}/g/b/z/ r-;",
        "7, 6, Move(10)    | x/r/b/z/r/b/x/ x/z/b/z/b- g/g/g/g/x- g/z/pi/z/pi- x/y/x/y/x- x- z-;",
        "5, 6, Move(10)    | x/r/x/r/x/ b/x/x/x/b/ x/x/g/x/x/ pi/x/x/x/pi/ x/y/x/y/x;",
        "5, 6, Move(10)    | x/r/x/r/x/ b/x/b/x/b/ x/g/x/g/x/ pi/x/pi/x/pi/ x/y/x/y/x;",
        "5, 6, Move(10)    | z/r/r/r/z/ y- b/b/z/b/b/ g- z/o/o/o/z;",
        "5, 6, Move(10)    | z/r/z/r/z/ y- z/b/z/b/z/ g- z/o/z/o/z;",
        "5, 6, Move(30)    | z/r/r/r/r/ y- z/b/b/b/b/ g- pi-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b{ba, wa(2)}/b*7/ g{ic}*3/g*5/ pi-;",
        "9, 6, Move(30)    | r*8/r{ic}/ o*7/o{ic}- y*6/y{ic}- g*5/g{ic}- c*4/c{ic}- b*3/b{ic}- p*2/p{ic}- pi/pi{ic}- m{ic}-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b{ba, wa(2)}/b*7/ g*4/g{ic}-/ pi-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b{ba, wa(2)}/b*7/ g*6/g{ic}*2/ pi-;",
        "6, 6, Move(30)    | p{ic}/o/p{ic}/o/p{ic}/o/ r/b{ic}/r/b{ic}/r/b{ic}/ p{ic}/o/p{ic}/o/p{ic}/o/ r/b{ic}/r/b{ic}/r/b{ic}/ p{ic}/o/p{ic}/o/p{ic}/o/ r/b{ic}/r/b{ic}/r/b{ic};",
        "5, 6, Move(30)    | z*5/ z/r/x/r/z/ z/z/r/z/z/ z/r/x/r/z/ z*5/;",
        "4, 6, Move(30)    | r*4/ o/b{ic(grid)}/o/b{ic(grid)}/ r*4/ o/p{ic(grid)}/o/p{ic(grid)};",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b{ba, wa(2)}/b*7/ g*7/g{ic, wa(2)}/ pi-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b*5/b{ba, wa(2)}/b/b/ g*4/g{ic, wa(2)}/g/g/g/ pi-;",
        "8, 6, Move(30)    | r- y- r*2/b/r/r/g/r*2/ p- pi- b*5/b{ba}/b/b/ g*4/g{ic}/g/g/g/ pi-;",
        "8, 6, Move(30)    | r- y- r*2/b{wa(1)}/r/r/g{wa(1)}/r*2/ p- pi- b*5/b/b/b/ g*4/g{ba}/g/g/g/ pi-;",
        "5, 6, Move(30)    | r- y- b/b/b{ba, ic}/b/b/ g- pi-;",
        "5, 6, Move(30)    | r- y- b/b/b{ic, ba}/b/b/ g- pi-;",
        "5, 6, Move(30)    | r- y- b/b/b{ic}/b/b{ic}/ g- pi-;",
        "5, 6, Move(30)    | b/b/b{ba}/b/b/ g/g{ba}/g/g/g/ pi- r- y-;",
        "5, 6, Move(30)    | r- y- b/b/b{ba}/b/b/ g/g{ba}/g/g/g/ pi-;",
        "5, 6, Move(30)    | r- y- b/b/b{ba}/b{ba}/b/ g- pi-;",
        "3, 6, Move(30)    | r- y{tl}/y/y{ol}/ b-;",
        "3, 6, Move(30)    | r- y/y{tv}/y{ol}/ b-;",
        "3, 6, Move(30)    | r- y/y{lb}/y{ol}/ b-;",
        "5, 6, Move(30)    | r- pi- y/y/y{ba}/y/y/ b/b/b{ba}/b/b/ o-;",
        "5, 6, Move(30)    | r- pi- y/y/y{ir(10)}/y/y/ b/b/b/b/b/ o-;",
        "5, 6, Move(30)    | r- pi- y/y/y{ca}/y/y/ b/b/b/b/b/ o-;",
        "5, 6, Move(30)    | r- pi- y/y/y/y/y/ b/b/b{ba}/b/b/ o-;",
        "7, 6, Move(30)    | r- y/y{tv}/y/y/y/y{ol}/y/ b-r-g-y-pi-;",
        "3, 6, Move(30)    | r- y/y{ol}/y{ol}/ b-;",
        "5, 6, Move(30)    | r- y- b/b{ch(r, b)}/b/b{ch(r, w)}/b/ g- m-;",
        "5, 6, Move(30)    | r- y- b*2/b{ch(r, b)}/b/b{ch(r, b)}/ g- m-;",
        "5, 6, Move(30)    | r- y- b/b/y/b/z/ g-  m/m/y/z/m;",
        "5, 6, Move(30)    | r- y- b/b{ch(r, b)}/b/b{ch(r, b)}/b/ g- m-;",
        "3, 6, Move(10)    | r- y- b/b/x;",
        "2, 6, Move(10)    | r- y-;",
        "5, 6, Move(10)    | r- y- b- g- m-;",
        "9, 6, Move(30)    | r- y- pi- p- b*2/b{ch(r, b)}/b*3/b{ch(r, b)}/b*2/ g- m- o- c-;",
        "5, 6, Move(30)    | r- y- b*2/b{ro(8, cw)}/b*2/ g- m-;",
        "5, 6, Move(30)    | r- y- b*2/b{ro(5, cw)}/b*2/ g- m-;",
        "5, 6, Move(30)    | r- y- b*2/b{ro(4, cw)}/b{ro(4, cw)}/b/ g*2/g{ro(4, cw)}/g{ro(4, cw)}/g/ pi-;",
        "5, 6, Move(30)    | r- y- b*2/b{d}/b*2/ g- m-;",
        "9, 6, Move(30)    | b*4/b{n}/b*4/ y- b*4/b{n}/b*4/ g- b*4/b{n}/b*4/ pi- b*4/b{n}/b*4/- o- b*4/b{n}/b*4/;",
        "5, 6, Move(10)    | r- y- b*2/b{n}/b*2/ g- m-;",
        "3, 6, Move(10)    | r{d}- y{d}/y/y{d}/ b{d}-;",
        "3, 6, Move(10)    | r{d}- y{d}- b{d}-;",
        "3, 6, Move(10)    | r- y- b-;",
        "3, 6, Move(30)    | r-b- b/<b{l(1), ca}, y{l(1), ba}>/<g{l(1), ir(19)}, p{l(1), n}>;",
        "3, 6, Move(30)    | r-b- b/<b{l(1)}, y{l(1)}>/<g{l(1)}, p{l(1)}>;",
        "3, 6, Move(30)    | r- y{ca}/y{l(1, 2)}/y{wa(1)}/ b-;",
        "6, 6, Move(30)    | r- y{d}/y{h}/y- pi- p/p{ch(ki, w)}/p/p/p{ch(q)}/p/ b-o-;",
        "6, 6, Move(30)    | r- y/y/y- pi- p/p{ch(ki, w)}/p/p/p{ch(q)}/p/ b-o-;",
        "9, 6, Move(30)    | r- y/y{d}/y- pi-p-b-o-g-r-y-;",
        "5, 6, Move(10)    | r{ch(p, w)}/r/pi{ch(r)}/pi/r{ch(kn, b)}/  g- p/p{ch(b, b)}/pi/pi{ch(ki, b)}/pi{ch(q, w)}/ m- y{ch(p)}/y{ch(r, b)}/pi/pi/y{ch(kn, b)};",
        "3, 6, Move(10)    | r- y/y/y/ b-;",
        "3, 6, Move(10)    | r { ro(2, cw) }     / r  { ro(2, cw), wa(1, grid) }      /  r { ro(2, cw) }     / " +
        "                    y { ro(4, cw), up } / pi { ro(4, cw), wa(1, grid), up }  /  p { ro(4, cw), up } / " +
        "                    b { ro(4, cw), up } / y  { ro(4, cw), up }               /  r { ro(4, cw), up };",
        "9x3, 6, Move(10)  | r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/r{ro(2, cw)}/ y- b-;",
        "3, 6, Move(10)    | r- y{ro(4, cw),up}/pi{ro(4, cw),up}/p{ro(4, cw),up}/ b{ro(4, cw),up}/y{ro(4, cw),up}/r{ro(4, cw),up};",
        "3, 6, Move(10)    | r{ro(2, cw)}/r{ro(2, cw)}/r/ y-b-;",
        "3, 6, Move(10)    | y-r/r{ro(2, cw)}/r{ro(2, cw)}/ b-;",
        "3, 6, Move(10)    | r- y/y{ro(2, cw)}/y{ro(2, cw)}/ b/b{ro(2, cw)}/b;",
        "3, 6, Move(10)    | r- y/pi{ro(4, cw),up}/p{ro(4, cw),up}/ b/y{ro(4, cw),up}/r{ro(4, cw),up};",
        "3, 6, Move(10)    | r{up}/r{up}/r{up}/ y{up}/y{ro(2, cw), up}/y{ro(2, cw), up}/ b{up}/b{ro(2, cw), up}/b{up};",
        "3, 6, Move(10)    | r/r/r/ y/y{ro(2, cw)}/y/ b/b/b{ro(2, cw)};",
        "3, 6, Move(10)    | r- y/y{ro(2, cw, grid)}/y{ro(2, cw, grid)}/ b/b{ro(2,cw, grid)}/b;",
        "5, 6, Move(10)    | r- y- b/b/z/b/b/ p-pi-;",
        "3, 6, Move(10)    | r- y/y{ro(2, cw)}/y/ b-;",
        "3, 6, Move(10)    | r{ro(1, cc), l(1, 2), up}/r/r/ b/b{ro(1, cw), l(1, 3), up}/b/ y/y{ro(1, cc, grid), l(2, 3), up}/y;",
        "3, 6, Move(10)    | r- y/y{ro(1, cc, grid), up}/y/ b-;",
        "3, 6, Move(10)    | r- y/y{ro(1, cc), up}/y/ b-;",
        "3, 6, Move(10)    | r- y/y{ro(1, cw), up}/y{ro(1, cc), ir(10)}/ b-;",
        "3, 6, Move(10)    | r/r/r/ y/y/y/ b/b/b;",
        "3, 6, Move(10)    | r- y/y/y/ b-;",
        "9x3, 6, Move(10)  | r- y- b-;",
        "5x3, 6, Move(10)  | r- y- b-;",
        "8x3, 6, Move(10)  | r- y- b-;",
        "7x3, 6, Move(10)  | r- y- b-;",
        "6x3, 6, Move(10)  | r- y- b-;",
        "4x3, 6, Move(10)  | r- y- b-;",
        "5, 6, Move(100)   | r/ o/<b {n(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/   " +
        "                    r/o/<b {n(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    <r{l(1, grid)}, r{n(grid), l(1, grid)}>/ <o{l(1, grid)}, o{n(grid), l(1, grid)}> / b{n} / <pi{l(1, grid)}, pi{n(grid), l(1, grid)}> / <p{l(1, grid)}, p{n(grid), l(1, grid)}>/    " +
        "                    r/o/<b {n(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    r/o/<b {n(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    ",
        "5, 6, Move(100)   | r/o/<b {z(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    r/o/<b {z(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    <r{l(1, grid)}, r{z(grid), l(1, grid)}>/ <o{l(1, grid)}, o{z(grid), l(1, grid)}> / b{z} / <pi{l(1, grid)}, pi{z(grid), l(1, grid)}> / <p{l(1, grid)}, p{z(grid), l(1, grid)}>/    " +
        "                    r/o/<b {z(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "5, 6, Move(100)   | r/o/<b {x(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    r/o/<b {x(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    <r{l(1, grid)}, r{x(grid), l(1, grid)}>/ <o{l(1, grid)}, o{x(grid), l(1, grid)}> / b{x} / <pi{l(1, grid)}, pi{x(grid), l(1, grid)}> / <p{l(1, grid)}, p{x(grid), l(1, grid)}>/    " +
        "                    r/o/<b {x(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    " +
        "                    r/o/<b {x(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    ",
        "                    r/o/<b {z(grid), l(1, grid)}, b{l(1, grid)}>/pi/p/    ",
        "5, 6, Move(10)    | r- b/b/<b{n(grid)}, b{ir(30, grid)}>/b/b/ y- pi- p-;",
        "5, 6, Move(10)    | r/r/<r, g>/r/r/ b/b/<b{n(grid)}, b{ir(30, grid)}>/b/b/ y- pi- p-;",
        "3, 6, Move(10)    | r-b/<b{ir(5, grid)}, b{ca(grid)}>/b/ y-;",
        "5, 6, Move(10)    | r-y- b/b/ <b{ir(10, grid)}, y{st(10, grid)}>/b/b/ pi-p-;",
        "7, 6, Move(10)    | r/r/r/z/y/y/y/ r/r{wa(1, grid), l(1, grid)}/r/z/y/y{wa(2, grid), l(1, grid)}/y/ r/r/r/z/y/y/y/ z- b/b/b/z/pi/pi/pi/ b/b{wa(2, grid), l(2, grid)}/b/z/pi/pi{wa(1, grid), l(2, grid)}/pi/ b/b/b/z/pi/pi/pi/ ",
        "7, 6, Move(10)    | r/r/r/z/y/y/y/ r/r{wa(1), l(1)}/r/z/y/y{wa(2), l(1)}/y/ r/r/r/z/y/y/y/ z- b/b/b/z/pi/pi/pi/ b/b{wa(2), l(2)}/b/z/pi/pi{wa(1), l(2)}/pi/ b/b/b/z/pi/pi/pi/ ",
        "3, 6, Move(10)    | r/<pi{l(1, grid)}, r{l(1, grid)}>/<r{l(1, grid)}, pi{l(1, grid)}>/ b/b/b/ y/y/y;",
        "7, 6, Move(10)    | r/r/r/z/y/y/y/ r/r{wa(1, grid)}/r/z/y/y{wa(2, grid)}/y/ r/r/r/z/y/y/y/ z- b/b/b/z/pi/pi/pi/ b/b{wa(2, grid)}/b/z/pi/pi{wa(1, grid)}/pi/ b/b/b/z/pi/pi/pi/ ",
        "5, 6, Move(10)    | r-y- b/b/b{wa(1, grid)}/b/b/ g-pi-;",
        "5, 6, Move(10)    | r-y- b/b/b{ca(grid)}/b/b/ g-pi-;",
        "5, 6, Move(10)    | r/r{wa(2)}/r/r{wa(1)}/r/  y- z- g- b/b{wa(1)}/b/b{wa(2)}/b;",
        "6, 6, Move(10)    | r*3/y*3/ r/r{wa(1)}/r/y- r*3/y*3/ b*3/g*3/ b*3/g/g{wa(1)}/g/ b*3/g*3;",
        "6, 6, Move(10)    | r*3/y*3/ r/r{st(3)}/r/y- r*3/y*3/ b*3/g*3/ b*3/g/g{ir(6)}/g/ b*3/g*3;",
        "6, 6, Move(10)    | r*3/y*3/ r/r{ca}/r/y/y{ca}/y/ r*3/y*3/ b*3/g*3/ b/b{ca}/b/g/g{ca}/g/ b*3/g*3;",
        "7, 6, Move(10)    | r-y-g- b*3/b{ca}/b*3/ pi-p-o-;",
        "7, 6, Move(10)    | r-y-g- b*3/b{ca}/b*3/ pi*3/pi{ir(7)}/pi*3 -p-o-;",
        "3, 6, Move(10)    | <r{n, l(1, 2)}, b{n, l(1, 2)}>/    <r{n, l(2, 3)},    b{n, l(2, 3)}>/        <r{n, l(1, 3)}, b{n, l(1, 3)}>/   " +
        "                    <r{n, l(3)},    b{n, l(3)}>/       <r{n, l(2, 3)},    b{n, l(2, 3)}>/        <r{n, l(1)},    b{n, l(1)}>/    " +
        "                    <r{n, l(1, 3)}, b{n, l(1, 3)}>/    <r{n, l(1, 2, 3)}, b{n, l(1, 2, 3)}>/     <r{n, l(1)},    b{n, l(1)}>;",
        "3, 6, Move(100)   | y- b/b{n, l(1, 2)}/b/ g-;",
        "3, 6, Move(10)    | r/r{ir(ca>)/r/ b/b/b/ y/y/y;",
        "3, 6, Move(10)    | r- y/y/y/ b-;",
        "3, 6, Move(10)    | <r{n, l(1, 2)}, b{n, l(1, 2)}>/<r{n, l(3)}, b{n, (3)}>/<r{n, l(1, 3)}, b{n, (1, 3)}>/    <r{n, l(3)}, b{n, l)>/<r{n, l(3)}, b{n, l(3)}>/<r{n, l(1)}, b{n, l(1)}>/    <r{n, l(1, 3)}, b{n, l(1, 3)}>/<r{n, l(1, 2, 3)}, b{n, l(1, 2, 3)}>/<r{n, l(1)}, b.{n, l(1)}>;",
        "3, 6, Move(10)    | <r.{n, l(1, 2)}, b.{n, l(1, 2)}>/<r.{n, l(3)}, b.{n, (3)}>/<r.{n, l(1, 3)}, b.{n, (1, 3)}>/    <r.{n, l.3}, b.{n, l)>/<r.{n, l(3)}, b.{n, l(3)}>/<r.{n, l.1}, b.{n, l.1}>/    <r.{n, l(1, 3)}, b.{n, l(1, 3)}>/<r.{n, l(1, 2, 3)}, b.{n, l(1, 2, 3)}>/<r.{n, l.1}, b.{n, l.1}>;",
        "3, 6, Move(10)    | r/<r, [ b>/r/ b/b/b/ y/y/y;",
        "3, 6, Move(10)    | r/r{ir(ca>)/r/ b/b/b/ y/y/y;",
        "3, 6, Move(10)    | r-b-   b/b/<b{wa(1), ir(7)}, r{wa(2), ir(3)}>;",
        "3, 6, Move(10)    | r-b-   b/b/b{wa(3), ir(7)};",
        "3, 6, Move(100)   | r- b/<b{ir(5), ca}, pi{ba}, r{l(1), wa(1)}, x>/b/ g-;",
        "3, 6, Move(100)   | r- b/<b{ir(5), ca}, pi{ba}, r{ca}, x>/b/ g-;",
        "3, 6, Move(100)   | r- b/<b{ir(5), ca}, pi{ba}>/b/ g-;",
        "3, 6, Move(100)   | r- b/<b{ir(5), ca}, pi{ir(ba)}>/b/ g-;",
        "3, 6, Move(100)   | r- b/<x, b>/<r, o>/ g-;",
        "3, 6, Move(100)   | r- b/<pi{ba}, y{ca}>/b/ g-;",
        "3, 6, Move(100)   | r- b/<pi, y>/b/ g-;",
        "3, 6, Move(10)    | r-b/b{ir(5), ca, ba}/b/ b-;",
        "3, 6, Move(10)    | r-b-b/b/<r, b.{ca, ir.5}[l, r, d]>;",
        "5, 6, Move(10)    | r-b- p/ <o.{l.1, ba, wa.2}, o.{l.1, ca, wa.2}>/ p /  <pi.{l.1, ir.5, wa.2}, pi.{l.1, ic, wa.2}>/p/ b-r-;",
        "5, 6, Move(10)    | r-b- p/ <o.{l.1, ba, wa.2}, o.{l.1, ca, wa.2}>/ p /  pi/p/ b-r-;",
        "5, 6, Move(10)    | r-b- p/ <b, o.{l.1, ca, wa.2}>/p/p/  pi/p/ b-r-;",
        "5, 6, Move(10)    | r-b- p/ <b, o.{l.1, ca}>/p/p/  pi/p/ b-r-;",
        "5, 6, Move(10)    | r-b- p/ o.{l.1, ba, wa.2}/o/p/  pi/p/ b-r-;",
        "3, 6, Move(10)    | r-b-   b/b/b.{ir.2, wa.2};",
        "3, 6, Move(10)    | r- b.wa.1/y.{ir.2, wa.2}/pi/ g.wa/y.{ir.2, wa.2};",
        "3, 6, Move(10)    | r- <b.l.1, o.l.1>/<b.l(1, 2), o.l(1, 2)>/<b.l.2, o.l.2>/ b-;",
        "3, 6, Move(10)    | r- <b.{n, l.1}, r.{ba, ca}>/b.{n, ca}/b.ca/ b-;",
        "4, 6, Move(100)   | r- z/<z, x, r>/x/<x, z>/ g- y-;",
        "4, 6, Move(100)   | r- z/<x, z>/x/<x, z>/ g- y-;",
        "3, 6, Move(10)    | r- b/b.{n, ca}/b.ca/ b-;",
        "3, 6, Move(10)    | r- <b.l.1, r.l.1>/<b.l.1, r.l.1>/b/ b-;",
        "3, 6, Move(10)    | r- <b.ir.5, r>/b/b/ b-;",
        "3, 6, Move(10)    | r- b.l.1/ r.l.1/ <o, b>/ b-;",
        "4, 6, Move(100)   | r/r/<o, p>/<p, o>/   b/<x, z>/<z, x>/b/ y- <x, pi>/pi-;",
        "4, 6, Move(100)   | r/r/<o, p>/<p, o>/   <g.ca, pi.ba>/<g.ca, pi.ba>/b- y- pi-;",

        // tested: 
        "3, 6, Move(10)    | r-b-b-;",
        "3, 6, Move(100)   | y- b- g-;",
        "3, 6, Move(7)     | r/b/r/ b- r/b/r;",
        "3, 6, Move(7)     | b/r/b/ r/b/r/ b/r/b;",
        "3, 6, Move(11)    | r-b-g-;",
        "3, 6, Move(8)     | r/g/b/ r/g*2/ b/r*2;",
        "3, 6, Move(8)     | r- b/x/b/ g-;",
        "3, 6, Move(10)    | r/x/r/ b/r/b/ g-;",
        "3, 6, Move(3)     | x/y/x/ r/b/r/ x/y/x;",
        "3, 6, Move(10)    | b- r/x/r/ g/x/g;",
        "3, 6, Move(15)    | r/b/g/ y- o/b/pi;",
        "3, 12, Move(8)    | y- g/z/g/ r-;",
        "4, 8, Move(10)    | r-b-r-b-;",
        "4, 6, Move(10)    | r*2/g*2/ r*2/g*2/ z/y/y/z/ y-;",
        "4, 8, Move(13)    | o- b*2/x/b/ r/x/r*2/ pi-;",
        "4, 8, Move(10)    | pi- z/r/x/r/ p/x/p/z/ g-;",               // Untested:
        "5, 6, Move(10)    | r- b- g/x/g{ro(9, cw), n}/z/g/ b- r-;",
        "4, 10, Move(14)   | r*2/b*2/ r*2/b*2/ y*2/g*2/ y*2/g*2;",
        "4, 8, Move(14)    | r/b/g/pi/ b/g/pi/r/ g/pi/r/b/ pi/b/g/r;",
        "4x3, 8, Move(10)  | r-b-g-;",
        "3x5, 10, Move(15) | y- r/z/r/ pi- b/z/b/ g-;",
        "6x3, 10, Move(15) | r- r/z/r/y/x/y/ b*3/pi*3;",
        "",
        "",
        "",
        "",
        "",
        "3, 6, Move(10)    | y/g.ca/y/ b/b.ca/b/ b-;",
        "3, 6, Move(10)    | r.ca/b/r.ca/ g- y.ca/b/y.ca;",
        "4, 8, Move(12)    | y- b/b.ca/z/g/ b/z/g.ca/g/ r-;",
        "6x3, 12, Move(15) | r/z/r/y- b- y*3/g/z/g;",
        "5, 10, Move(15)   | r- r/b.ca/g/y.ca/r/ r/b*3/r/ r/pi.ca/g/p.ca/r/ r-;",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "3, 6, Move(13)   | r/b/y/ r/b.ir.5/y/ pi-;",
        "3, 6, Move(13)   | y/b/g.ir.6/ b/g.ir.6/r/ g.ir.6/r/r;",
        "4x3, 7, Move(13) | y/x/r/b.ir.4/ y/p/r/b;",
        "3x5, 8, Move(13) | p/z/p/ b- r/r.ir.8/r/ o- p/z/p;",
        "4, 8, Move(13)   | r/r/y/y/ g/x/x/g/ b/b/o/o;",
        "4, 8, Move(18)   | y/y/y/r/ g/g/g.{ir.8, ca}/r/ p/p/p/r/ b/b.{ir.9, ca}/b/r;",
    };
}

// Unsorted:
/*
 * r*3/y.ir.6/pi- r*3/y.ir.6/pi- pi*2/y.ir.3/b- pi*2/y.ir.3/b-
 * r*3/x/o- r*3/x/o- pi*2/z/b- pi*2/z/b-
 */
