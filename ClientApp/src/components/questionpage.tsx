import React, { useState } from 'react'
import {useParams} from 'react-router-dom'
import {Page} from './page'
import {getQuestion,QuestionData} from './questiondata'
import {AnswerList} from './answerlist'
import {useSelector,useDispatch} from 'react-redux'
import {gettingQuestionAction,gotQuestionAction,AppState} from './store'


export const QuestionPage =()=>{
 
    const dispatch=useDispatch()
const {questionId}=useParams<{questionId?:string}>()

const question=useSelector((state:AppState)=>{
  return state.questions.viewing
  })
React.useEffect(() => {
 
  

    const doGetQuestion = async (
     
      questionId: number,

    ) => {
      dispatch(gettingQuestionAction)
      const foundQuestion= await getQuestion(

        questionId,

      );
      dispatch(gotQuestionAction(foundQuestion))
   
    };

    if (questionId) {

       doGetQuestion(Number(questionId));
     


    }
   
  }, [questionId]);


    return (
        <div>
            <Page>
               {  question !== null &&  question !== undefined ?
               <React.Fragment>
               <p>{question.title}</p>
               <p>{question.content}</p>
               <div >
      {`Asked by ${question.userName} on
  ${question.created.toLocaleDateString()}
  ${question.created.toLocaleTimeString()}`}

    </div>
    <AnswerList data={question.answers} />
               </React.Fragment>  : ""  }
            </Page>
        </div>
    )
}