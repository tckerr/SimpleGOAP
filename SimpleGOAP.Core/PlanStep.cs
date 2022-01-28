namespace SimpleGOAP
{
    public class PlanStep<T>
    {
        public int Index { get; set; }
        public IAction<T> Action { get; set; }
        public T BeforeState {get;set;}
        public T AfterState {get;set;}
    }
}
