//using System;
//using System.Linq.Expressions;
//using System.Security.Principal;
//using System.Threading;
//
//using SystemSupport.Web.Config;
//
//namespace DecisionCritical.UnitTests.Web
//{
//    using CC.Core.DomainTools;
//
//    using KnowYourTurf.Core.Domain;
//
//    public class ControllerTester<ACTOR, INPUT, OUTPUT>
//        where ACTOR : class
//        where OUTPUT : class
//    {
//        private readonly Expression<Func<ACTOR, INPUT, OUTPUT>> _expression;
//     
//        #region private members
//        private readonly Func<ACTOR, INPUT, OUTPUT> _func;
//        private OUTPUT _output;
//        private INPUT _input;
//        private IPrincipal _previousPrincipal;
//        private RhinoAutoMocker<ACTOR> _services;
//
//        #endregion
//
//        #region public properties
//        protected bool UseInMemoryRepoAndSaveService { get; set; }
//        protected bool UseDummyValidator { get; set; }
//        protected User TheCurrentUser { get; set; }
//        public INPUT Given
//        {
//            set { _input = value; }
//            get { return _input; }
//        }
//
//        public OUTPUT Output
//        { get { return _output ?? getOutput(); } }
//
//        public RhinoAutoMocker<ACTOR> Services { get { return _services; } }
//        #endregion
//
//        public ControllerTester(Expression<Func<ACTOR, INPUT, OUTPUT>> expression)
//        {
//            _expression = expression;
//            _func = _expression.Compile();
//        }
//
//        protected void TheCurrentUserIs(User user)
//        {
//            TheCurrentUser = user;
//            _repo = MockFor<IRepository>();
//            _repo.Stub(r => r.Find<User>(user.EntityId)).Return(user);
//            var identity = new GenericIdentity(user.EntityId.ToString());
//
//            Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[]{}); 
//        }
//
//        public T MockFor<T>() where T : class
//        {
//            return _services.Get<T>();
//        }
//
//        public IMethodOptions<R> MockFor<T, R>(Function<T, R> func) where T : class
//        {
//            return MockFor<T>().Expect(func);
//        }
//
//
//        protected virtual void beforeEach()
//        { }
//
//        protected virtual void afterEach()
//        { }
//
//        [SetUp]
//        public virtual void Setup()
//        {
//            _previousPrincipal = Thread.CurrentPrincipal;
//            //Bootstrapper.Bootstrap();
//            Bootstrapper.BootstrapTest();
//            _services = new RhinoAutoMocker<ACTOR>(MockMode.AAA);
//
//            // _services.Inject(ObjectFactory.GetInstance<IUrlResolver>());
//            _output = null;
//
//            beforeEach();
//        }
//
//
//
//        public void VerifyCallsTo<T>() where T : class
//        {
//            TriggerOutputIfNotAlreadyTriggered();
//            MockFor<T>().VerifyAllExpectations();
//        }
//
//
//        protected OUTPUT TriggerOutputIfNotAlreadyTriggered()
//        {
//            return Output;
//        }
//
//        protected OUTPUT getOutput()
//        {
//            _output = _func(_services.ClassUnderTest, _input);
//            return _output;
//        }
//
//        public void ClearResults()
//        {
//            _output = null;
//        }
//
//        [TearDown]
//        public void TearDown()
//        {
//            Thread.CurrentPrincipal = _previousPrincipal;
//            ObjectFactory.ResetDefaults();
//            afterEach();
//        }
//
//    }
//}
//
//public class ControllerTester<ACTOR, OUTPUT>
//    where ACTOR : class
//    where OUTPUT : class
//{
//    private readonly Expression<Func<ACTOR, OUTPUT>> _expression;
//
//    #region private members
//    private readonly Func<ACTOR, OUTPUT> _func;
//    private OUTPUT _output;
//    private IPrincipal _previousPrincipal;
//    private RhinoAutoMocker<ACTOR> _services;
//
//    #endregion
//
//    #region public properties
//    protected bool UseInMemoryRepoAndSaveService { get; set; }
//    protected bool UseDummyValidator { get; set; }
//    protected User TheCurrentUser { get; set; }
//
//    public OUTPUT Output
//    { get { return _output ?? getOutput(); } }
//
//    public RhinoAutoMocker<ACTOR> Services { get { return _services; } }
//    #endregion
//
//    public ControllerTester(Expression<Func<ACTOR, OUTPUT>> expression)
//    {
//        _expression = expression;
//        _func = _expression.Compile();
//    }
//
//    protected void TheCurrentUserIs(User user)
//    {
//        TheCurrentUser = user;
//        IRepository _repo;
//        _repo = MockFor<IRepository>();
//        _repo.Stub(r => r.Find<User>(user.EntityId)).Return(user);
//        var identity = new GenericIdentity(user.EntityId.ToString());
//
//        Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[] { });
//    }
//
//    public T MockFor<T>() where T : class
//    {
//        return _services.Get<T>();
//    }
//
//    public IMethodOptions<R> MockFor<T, R>(Function<T, R> func) where T : class
//    {
//        return MockFor<T>().Expect(func);
//    }
//
//
//    protected virtual void beforeEach()
//    { }
//
//    protected virtual void afterEach()
//    { }
//
//    [SetUp]
//    public virtual void Setup()
//    {
//        _previousPrincipal = Thread.CurrentPrincipal;
//        StructureMapBootstrapperTesting.Bootstrap();
//        _services = new RhinoAutoMocker<ACTOR>(MockMode.AAA);
//
//        // _services.Inject(ObjectFactory.GetInstance<IUrlResolver>());
//        _output = null;
//
//        beforeEach();
//    }
//
//
//
//    public void VerifyCallsTo<T>() where T : class
//    {
//        TriggerOutputIfNotAlreadyTriggered();
//        MockFor<T>().VerifyAllExpectations();
//    }
//
//
//    protected OUTPUT TriggerOutputIfNotAlreadyTriggered()
//    {
//        return Output;
//    }
//
//    protected OUTPUT getOutput()
//    {
//        return _func(_services.ClassUnderTest);
//    }
//
//    public void ClearResults()
//    {
//        _output = null;
//    }
//
//    [TearDown]
//    public void TearDown()
//    {
//        Thread.CurrentPrincipal = _previousPrincipal;
//        ObjectFactory.ResetDefaults();
//        afterEach();
//    }
//
//
//}