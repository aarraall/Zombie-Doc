using System;

[Serializable]
public abstract class Data
{
    protected string _persistentPath; public string PersistentPath => _persistentPath;
    public abstract void SetDefault();
    public abstract void SetData<TData>(TData data) where TData : Data;

}