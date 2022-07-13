namespace Microservices.GeneratingChaos.BuildingBlocks.Helpers
{
    /// <summary>
    /// Base command execution response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCommandResponse<T>
    {
        /// <summary>
        /// Gets or sets the response dto.
        /// </summary>
        /// <value>The response dto.</value>
        public T ResponseDto { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BaseCommandResponse" /> is succeed.
        /// </summary>
        /// <value><c>true</c> if succeed; otherwise, <c>false</c>.</value>
        public bool Succeed { get; private set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        public string Error { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandResponse" /> class.
        /// </summary>
        protected BaseCommandResponse() : this(true, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandResponse" /> class.
        /// </summary>
        /// <param name="error">The error.</param>
        protected BaseCommandResponse(string error) : this(false, error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandResponse" /> class.
        /// </summary>
        /// <param name="succeed">if set to <c>true</c> [succeed].</param>
        /// <param name="error">The error.</param>
        protected BaseCommandResponse(bool succeed, string error)
        {
            Succeed = succeed;
            Error = error;
        }
    }
}
