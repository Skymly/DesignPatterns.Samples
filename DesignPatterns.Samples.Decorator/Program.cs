using Decorator.Sample;
using DesignPatterns.Structural;

var core = new PaymentService();

Console.WriteLine("Generated order constants:");
Console.WriteLine($"  LoggingPaymentDecorator = {PaymentServiceDecoratorOrder.LoggingPaymentDecorator}");
Console.WriteLine($"  TimingPaymentDecorator = {PaymentServiceDecoratorOrder.TimingPaymentDecorator}");
Console.WriteLine();

var decorated = PaymentServiceDecoratorStack.Build(core);

Console.WriteLine("Core only:");
Console.WriteLine(core.Pay("card", 42m));

Console.WriteLine();
Console.WriteLine("Decorated stack (log outer, timing inner):");
Console.WriteLine(decorated.Pay("card", 42m));

Console.WriteLine();
var enableTiming = !string.Equals(Environment.GetEnvironmentVariable("ENABLE_TIMING"), "0", StringComparison.Ordinal);
Console.WriteLine($"Conditional stack (timing enabled={enableTiming}):");
var conditional = new DecoratorStackBuilder<IPaymentService>()
    .Add<LoggingPaymentDecorator>()
    .Add<TimingPaymentDecorator>(() => enableTiming)
    .Build(new PaymentService());
Console.WriteLine(conditional.Pay("card", 42m));
