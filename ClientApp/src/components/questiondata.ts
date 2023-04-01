import {http} from './http'


export interface QuestionData {
    questionId: number;
    title: string;
    content: string;
    userName: string;
    created: Date;
    answers: AnswerData[];
  }
  export interface AnswerData {
    answerId: number;
    content: string;
    userName: string;
    created: Date;
  }
  export interface QuestionDataFromServer {
    questionId: number;
    title: string;
    content: string;
    userName: string;
    created: string;
    answers: AnswerData[];
  }

  
  export const getUnansweredQuestions = async (): Promise<QuestionData[]> => {
     
     const result = await http <QuestionDataFromServer[]>({path:'/questions/unanswered'})
     if (result.ok && result.body){
    return  result.body.map (res=> ({
      ...res,
      created:new Date(res.created)
    }))
   
     }
     
     else {
      return []
    }         
   
  };
  
  const wait = async (ms: number): Promise<void> => {
    return new Promise((resolve) => setTimeout(resolve, ms));
  };
  
  export const getQuestion = async (
    questionId: number,
  ): Promise<QuestionData | null> => {
   const result =await http <QuestionDataFromServer>({path:`/questions/${questionId}`})
   if (result.ok && result.body) {
  return {...result.body,created:new Date(result.body.created)}
   }
   else {
     return null ;
   }
  };
  
  export const searchQuestions = async (
    criteria: string,
  ): Promise<QuestionData[]> => {
    const result= await http<QuestionDataFromServer []>({path:`/questions?search=${criteria}`})
    if (result.ok && result.body){
      return result.body.map(res=> ({
        ...res,
        created:new Date(res.created)
      }))
    }
    else {
      return []
    }
  };

export interface PostQuestionData {

  title: string;

  content: string;

  userName: string;

  created: Date;

}

export interface PostAnswerData {

  questionId: number;

  content: string;

  userName: string;

  created: Date;

}