using SlurmFortress.Core.Helpers.Exceptions;

namespace SlurmFortress.Core.Slurms.Operations;

public sealed class SlurmUpdateOperation
{
    public SlurmUpdateContext Context { get; }

    public SlurmUpdateOperation(SlurmUpdateContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Load()
    {
        Context.Entity = Context.Loader.Get(Context.Id).ValidateFound(Context.Id);
    }

    public void Validate()
    {
        if (Context.Entity == null)
            throw new InvalidOperationException($"You must call {nameof(Load)} before {nameof(Validate)}");
    }

    public void StageChanges()
    {
        if (Context.Entity is null)
            throw new InvalidOperationException($"Must call {nameof(Load)} before {nameof(StageChanges)}");

        Context.UpdateFunc(Context.Entity);

        //here would be some more complex logic like updating child objects or what have you... 
        // things that need to be unit tested.
        Context.DataTransaction.Update(Context.Entity);
    }
}
