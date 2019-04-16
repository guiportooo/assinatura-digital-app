using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class TermsOfUseServiceFake : ITermsOfUseServices
    {
        const string termsOfUse =
            @"
                Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.
                Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.

                Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.
                Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.

                Maecenas aliquam massa id malesuada porttitor.
                Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.
                Phasellus sed neque sem. Nulla facilisi.

                Maecenas aliquam massa id malesuada porttitor.
                Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.
                Phasellus sed neque sem. Nulla facilisi.

                Vestibulum sodales tortor quis risus sodales, eget ornare nisl mattis.
                Nam varius tellus mi, eu commodo lectus eleifend sit amet.
                Morbi consequat nibh ligula, in molestie risus placerat condimentum.
                Nunc facilisis felis in massa aliquam dignissim.

                Vestibulum sodales tortor quis risus sodales, eget ornare nisl mattis.
                Nam varius tellus mi, eu commodo lectus eleifend sit amet.
                Morbi consequat nibh ligula, in molestie risus placerat condimentum.
                Nunc facilisis felis in massa aliquam dignissim.

                Vestibulum sodales tortor quis risus sodales, eget ornare nisl mattis.
                Nam varius tellus mi, eu commodo lectus eleifend sit amet.
                Morbi consequat nibh ligula, in molestie risus placerat condimentum.
                Nunc facilisis felis in massa aliquam dignissim.

                Vestibulum sodales tortor quis risus sodales, eget ornare nisl mattis.
                Nam varius tellus mi, eu commodo lectus eleifend sit amet.
                Morbi consequat nibh ligula, in molestie risus placerat condimentum.
                Nunc facilisis felis in massa aliquam dignissim.
             ";

        public Task<string> GetTermsUse() => Task.FromResult(termsOfUse);

    }
}
