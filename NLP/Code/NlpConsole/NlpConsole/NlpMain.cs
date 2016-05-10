﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlpConsole
{
    class NlpMain
    {
        public NlpMain()
        {
            string text = @"Another    ex-  Golden Stater, Paul Stankowski from Oxnard, is contending
for a berth on the U.S. Ryder Cup team after winning his first PGA Tour
event last year and staying within three strokes of the lead through
three rounds of last month's U.S. Open. H.J. Heinz Company said it
completed the sale of its Ore-Ida frozen-food business catering to the
service industry to McCain Foods Ltd. for about $500 million.
It's the first group action of its kind in Britain and one of
only a handful of lawsuits against tobacco companies outside the 
U.S. A Paris lawyer last year sued France's Seita SA on behalf of
two cancer-stricken smokers. Japan Tobacco Inc. faces a suit from
five smokers who accuse the government-owned company of hooking
them on an addictive product.";

            //var tokens=new Tokenizer().GetTokens(text);
            //foreach (var token in tokens)
            //{
            //    Console.WriteLine(token);
            //}
            new MassiveDataExtractor().Extract(text);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            new NlpMain();
        }
    }
}
