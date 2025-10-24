namespace community.tests.common;

/// <summary>
/// Instantiates the supplied entry point for engaging an API for integration testing
/// </summary>
/// <typeparam name="T"></typeparam>
public class TestApplicationFactory<T> : WebApplicationFactory<T>
    where T : class;