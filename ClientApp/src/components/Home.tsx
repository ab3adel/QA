import * as React from 'react';
import {QuestionList} from './questionlist'
import {Container,Row} from 'reactstrap'
import {useDispatch,useSelector} from 'react-redux'
import {AppState,gotUnAnsweredQuestionsAction,gettingUnAnsweredQuestionsAction} from './store'
import {getUnansweredQuestions} from './questiondata'


export const Home = () => {
  const dispatch=useDispatch()
  const questions = useSelector((state:AppState)=>{
    return state.questions.unanswerd
  })
  const questionLoading =useSelector((state:AppState)=>{
  return state.questions.loading
  })
  React.useEffect(() => {

    const doGetUnansweredQuestions = async () => {
  
      dispatch(gettingUnAnsweredQuestionsAction());
  
      const unansweredQuestions = await getUnansweredQuestions();
     dispatch(gotUnAnsweredQuestionsAction(unansweredQuestions))
    };
  
    doGetUnansweredQuestions();
  
  }, [])
 
  return (
  <div>
 <button >navigate to sign in </button>
 <Container>
   {questions[0] ? <QuestionList data={questions}/>:"no answers to show"}
 </Container>
  </div>
);

  }
