import React from 'react'
import { QuestionData } from './questiondata';
import { Question } from './question';
import {Row} from 'reactstrap'
interface Props {
    data: QuestionData[];
  }
  
export const QuestionList =({data}:Props)=>{
    return (
<React.Fragment>
    {data.map((question) => (
      <Row
        key={question.questionId}>
        <Question data={question} />
      </Row>
    ))}
  </React.Fragment>
    )
}