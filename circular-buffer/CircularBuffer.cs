using System;

public class CircularBuffer<T>
{
    //////////////////////////////////////////////////////
    // You can add your own methods but .:[DO NOT]:.
    // change the signature of the provided methods
    //////////////////////////////////////////////////////

    private T[] Buffer;

    private int _newPosition;

    private int _oldPosition;

    private bool _isFull;

    public int Length;

    public CircularBuffer(int capacity)
    {
        Buffer = new T[capacity];
        _newPosition = 0;
        _oldPosition = 0;
        Length = capacity;
    }

    public T this[int index]
    {
        get { return Buffer[index]; }
    }

    public T Read()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Buffer is empty! Empty buffer is not readable!");
        }

        //Reading oldest element and making position vacant for further use

        T result = Buffer[_oldPosition];

        Buffer[_oldPosition] = default(T);

        UpdateOldPosition();

        _isFull = false;

        return result;
    }

    public void Write(T value)
    {
        if (_isFull)
        {
            throw new InvalidOperationException("Buffer is full! Either overwrite or clear the buffer.");
        }

        Buffer[_newPosition] = value;

        UpdateNewPosition();

        if (_newPosition == _oldPosition)
        {
            _isFull = true;
        }
    }

    public void Overwrite(T value)
    {
        if (!_isFull)
        {
            Write(value);
        }
        else
        {
            Buffer[_newPosition] = value;

            //Updating the position of both new and old for further overwrite or read operations
            UpdateNewPosition();
            UpdateOldPosition();
        }
    }

    public void Clear()
    {
        _oldPosition = 0;
        _newPosition = 0;
        _isFull = false;

        Buffer = new T[Buffer.Length];
    }

    private bool IsEmpty()
    {
        if (!_isFull && (_oldPosition == _newPosition))
        {
            return true;
        }
        return false;
    }

    private void UpdateOldPosition()
    {
        _oldPosition++;
        if (_oldPosition == Buffer.Length)
        {
            _oldPosition = 0;
        }
    }

    private void UpdateNewPosition()
    {
        _newPosition++;
        if (_newPosition == Buffer.Length)
        {
            _newPosition = 0;
        }
    }
}