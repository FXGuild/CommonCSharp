// MIT License
//
// Copyright (c) 2016 FXGuild
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using FXGuild.Common.Tracing.TracedTestApp.Common;
using System.Collections.Generic;
using System.Linq;

namespace FXGuild.Common.Tracing.TracedTestApp.Cultivation
{
    internal sealed partial class Cultivator
    {
        #region Methods

        internal List<CoffeeCherry> Cultivate()
        {
            var speciesSelector = new CoffeeSpeciesSelector();
            var species = speciesSelector.Select();

            var germinators = new List<Germinator>();
            var seedBeds = new List<SeedBed>();
            for (uint i = 0; i < 100; ++i)
            {
                germinators.Add(new Germinator());
                seedBeds.Add(new SeedBed());
            }

            var seedlings = germinators.Select(a_G => a_G.Germinate(species)).ToList();

            var trees = seedlings.Select((a_T, a_I) => seedBeds[a_I].Develop(a_T)).ToList();

            var field = new Field();
            field.RemoveWeeds();
            field.Fertilize();
            field.Grow(trees);

            var cherries = new List<CoffeeCherry>();
            foreach (var tree in trees)
            {
                cherries.AddRange(Harvest(tree));
            }

            return cherries;
        }

        private List<CoffeeCherry> Harvest(CoffeeTree a_Tree)
        {
            if (!a_Tree.IsMature)
            {
                return new List<CoffeeCherry>();
            }

            var cherries = new List<CoffeeCherry>();
            for (uint i = 0; i < 30; ++i)
            {
                cherries.Add(new CoffeeCherry());
            }
            return cherries;
        }

        #endregion
    }
}