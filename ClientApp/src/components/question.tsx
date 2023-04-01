import React from 'react'
import {QuestionData} from './questiondata'
interface Props {data:QuestionData,showContent?:boolean}
export const Question =({data,showContent}:Props)=>{
    return (
       <ul>
         
           {data.content?.length > 50 ?
           <li>{`${data.content.substring(0,50)}....`}</li>:
           <li>{data.content}</li>}
          {`Asked by ${data.userName} on
        ${data.created.toLocaleDateString()} ${data.created.toLocaleTimeString()}`}
       </ul>
    )
}