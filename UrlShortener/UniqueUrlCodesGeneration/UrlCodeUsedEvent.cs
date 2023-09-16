namespace UrlShortener.UniqueUrlCodesGeneration
{
    public sealed class UrlCodeUsedEvent
    {
        public event EventHandler? CodeUsed;

        //public void OnCodeUsed()
        //{
        //    CodeUsed?.Invoke(this, EventArgs.Empty);
        //}

        public async Task OnCodeUsed()
        {
            if (CodeUsed != null)
            {
                foreach (var handler in CodeUsed.GetInvocationList().Cast<Func<Task>>())
                {
                    await handler();  // This line should not block if handlers are truly asynchronous
                }
            }
        }

    }

}
