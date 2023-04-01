using QA.Model;
namespace QA.Data
{
   public interface IQuestionCache {
        QuestionGetSingleResponse Get (int quesitonId);
        void Set (QuestionGetSingleResponse question);
        void Remove (int questionId);
    }
}