﻿using System.Collections.Generic;
using System.Linq;

namespace DidacticalEnigma.Core.Models.LanguageService
{
    sealed class NullSimilarKana : ISimilarKana
    {
        public IEnumerable<CodePoint> FindSimilar(CodePoint point)
        {
            return Enumerable.Empty<CodePoint>();
        }

        private NullSimilarKana()
        {

        }

        public static NullSimilarKana Instance { get; } = new NullSimilarKana();
    }
}
