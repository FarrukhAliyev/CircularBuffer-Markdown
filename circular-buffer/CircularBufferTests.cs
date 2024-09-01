using System;
using Xunit;

public class CircularBufferTests
{
    /////////////////////////////////////////////////////////
    // You can add more tests to validate your implementation
    /////////////////////////////////////////////////////////

    [Fact]
    public void Reading_empty_buffer_should_fail()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        Assert.Throws<InvalidOperationException>(() => buffer.Read());
    }

    [Fact]
    public void Can_read_an_item_just_written()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        buffer.Write(1);
        Assert.Equal(1, buffer.Read());
    }

    [Fact]
    public void Writing_inside_full_buffer_should_fail()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        buffer.Write(1);
        Assert.Throws<InvalidOperationException>(() => buffer.Write(2));
    }

    [Fact]
    public void Can_overwrite_inside_not_full_buffer()
    {
        var buffer = new CircularBuffer<int>(capacity: 3);
        buffer.Overwrite(1);
        Assert.Equal(1, buffer.Read());
    }
}